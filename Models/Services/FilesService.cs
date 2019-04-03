using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models.DatabaseEntities;
using Models.Extensions;
using Models.Interfaces.Repository;
using Models.Models;
using Utilities;

namespace Models.Services
{
    public class FilesService : BaseService
    {
        private readonly int _initialFileVersion = 1;
        private readonly int _defaultFileStreamBufferSize = 4096;
        private readonly IDictionary<string, string> FileExtensionToContentFileName;
        private readonly IFilesRepository _filesRepository;
        private readonly IGlossaryRepository _glossaryRepository;
        private readonly ILocaleRepository _localeRepository;
        private readonly IFilesPackagesRepository _filesPackagesRepository;

        public event Func<string, FailedFileParsingModel, Task> FileParsingFailed;

        public FilesService(
            IFilesRepository filesRepository,
            IGlossaryRepository glossaryRepository,
            ILocaleRepository localeRepository,
            IFilesPackagesRepository filesPackagesRepository,
            SettingsModel settings
            )
        {
            this._filesRepository = filesRepository;
            this._glossaryRepository = glossaryRepository;
            this._localeRepository = localeRepository;
            this._filesPackagesRepository = filesPackagesRepository;
            this.FileExtensionToContentFileName = settings.Files.FileExtensionToContentFileName;
        }

        public async Task<IEnumerable<Node<File>>> GetAllAsync(Guid? userId, Guid? projectId)
        {
            var files = await this._filesRepository.GetAllAsync(userId, projectId);
            return files?.ToTree((file, icon) => new Node<File>(file, icon), (file) => GetIconByFile(file));
        }

        public async Task<IEnumerable<Node<File>>> GetByProjectIdAsync(Guid projectId, string fileNamesSearch)
        {
            var files = await this._filesRepository.GetByProjectIdAsync(projectId: projectId, fileNamesSearch: fileNamesSearch);
            if (string.IsNullOrEmpty(fileNamesSearch))
            {
                return files.ToTree((file, icon) => new Node<File>(file, icon), (file) => this.GetIconByFile(file));
            }
            else
            {
                var idsToFiles = files.ToDictionary(keySelector: value => value.id);
                var parentsIds = idsToFiles.Where(x => x.Value.id_folder_owner != null
                                                    && !idsToFiles.ContainsKey(x.Value.id_folder_owner.Value))
                                           .Select(x => x.Value.id_folder_owner)
                                           .ToList();
                do
                {
                    var newParentsIds = new List<Guid?>();
                    foreach (var parentId in parentsIds)
                    {
                        var parentFile = await this._filesRepository.GetByIDAsync(parentId);
                        idsToFiles[parentFile.id] = parentFile;
                        if (parentFile.id_folder_owner != null
                            && !idsToFiles.ContainsKey(parentFile.id_folder_owner.Value))
                        {
                            newParentsIds.Add(parentFile.id_folder_owner);
                        }
                    }
                    parentsIds = newParentsIds;
                } while (parentsIds.Any());
                return idsToFiles.Values.ToTree((file, icon) => new Node<File>(file, icon), (file) => this.GetIconByFile(file));
            }
        }

        public async Task<File> GetByIdAsync(Guid id)
        {
            return await this._filesRepository.GetByIDAsync(id);
        }

        public IEnumerable<File> GetInitialFolders(Guid projectId)
        {
            return this._filesRepository.GetInitialFolders(projectId);
        }

        public async Task<Node<File>> AddFileAsync(string fileName, System.IO.Stream fileContentStream, Guid? parentId, Guid projectId)
        {
            var foundedFile = await this._filesRepository.GetLastVersionByNameAndParentIdAsync(fileName, parentId, projectId);
            if (foundedFile != null)
            {
                throw new Exception(WriteLn($"Файл \"{fileName}\" уже есть."));
            }

            var (newFile, filePackage) = await this.GetNewFileModelAsync(
                fileName: fileName,
                fileContentStream: fileContentStream);
            newFile.id_folder_owner = parentId;
            newFile.id_localization_project = projectId;

            return await this.AddNodeAsync(newFile, insertToDbAction: file => this.InsertFileToDbAsync(file, filePackage), projectId: projectId);
        }

        public async Task<Node<File>> UpdateFileVersionAsync(string fileName, System.IO.Stream fileContentStream, Guid? parentId, Guid projectId)
        {
            var version = this._initialFileVersion;
            var lastVersionDbFile = await this._filesRepository.GetLastVersionByNameAndParentIdAsync(fileName, parentId, projectId);
            if (lastVersionDbFile != null)
            {
                if (lastVersionDbFile.is_folder)
                {
                    throw new Exception(WriteLn("Нельзя обновить папку " + lastVersionDbFile.name_text));
                }

                lastVersionDbFile.is_last_version = false;
                if (!lastVersionDbFile.version.HasValue)
                {
                    lastVersionDbFile.version = this._initialFileVersion;
                }
                version = lastVersionDbFile.version.Value + 1;

                // TODO: single transaction?
                var updatedSuccessfully = await this._filesRepository.UpdateAsync(lastVersionDbFile);
                //TODO нужно копировать переводы в новую версию
                if (!updatedSuccessfully)
                {
                    throw new Exception(WriteLn("Не удалось обновить старый файл."));
                }
            }

            var (newVersionFile, filePackage) = await this.GetNewFileModelAsync(
                fileName: fileName,
                fileContentStream: fileContentStream);
            newVersionFile.id_folder_owner = parentId;
            newVersionFile.id_localization_project = projectId;
            newVersionFile.version = version;
            newVersionFile.id_previous_version = lastVersionDbFile?.id;
            newVersionFile.download_name = lastVersionDbFile?.download_name;
            newVersionFile.translator_name = lastVersionDbFile?.translator_name;

            var newNode = await this.AddNodeAsync(newVersionFile, insertToDbAction: file => this.InsertFileToDbAsync(file, filePackage), projectId: projectId);

            if (lastVersionDbFile != null)
            {
                var localesIds = (await this._filesRepository.GetLocalesForFileAsync(fileId: lastVersionDbFile.id))
                                 .Select(x => x.id);
                await this.UpdateTranslationLocalesForFileAsync(fileId: newNode.Data.id, localesIds: localesIds);
            }
            return newNode;
        }

        public async Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoAsync(Guid fileId)
        {
            var file = await this._filesRepository.GetByIDAsync(id: fileId);
            if (file.is_folder)
            {
                var childTranslationInfos = new List<FileTranslationInfo>();
                var currentLevelFiles = new List<File>() { file };
                while (currentLevelFiles.Any())
                {
                    var newLevelFiles = new List<File>();
                    foreach (var currentLevelFile in currentLevelFiles)
                    {
                        if (currentLevelFile.is_folder)
                        {
                            newLevelFiles.AddRange(await this._filesRepository
                                .GetFilesByParentFolderIdAsync(parentFolderId: currentLevelFile.id));
                        }
                        else
                        {
                            childTranslationInfos.AddRange(await this._filesRepository
                                .GetFileTranslationInfoByIdAsync(fileId: currentLevelFile.id));
                        }
                    }
                    currentLevelFiles = newLevelFiles;
                }

                var folderTranslationInfos = new List<FileTranslationInfo>();
                var groupedByLocale = childTranslationInfos.GroupBy(x => x.LocaleId);
                foreach (var localeIdToTranslationInfos in groupedByLocale)
                {
                    var localeId = localeIdToTranslationInfos.Key;
                    var translationInfos = localeIdToTranslationInfos.ToArray();
                    var filesCount = translationInfos.Length;
                    var averageConfirmed = translationInfos.Sum(x => x.PercentOfConfirmed) / filesCount;
                    var averageTranslated = translationInfos.Sum(x => x.PercentOfTranslation) / filesCount;
                    folderTranslationInfos.Add(new FileTranslationInfo()
                    {
                        LocaleId = localeId,
                        PercentOfConfirmed = averageConfirmed,
                        PercentOfTranslation = averageTranslated
                    });
                }
                return folderTranslationInfos;
            }
            else
            {
                return await this._filesRepository.GetFileTranslationInfoByIdAsync(fileId: file.id);
            }
        }

        /// <summary>
        /// Создаем StreamReader
        /// </summary>
        /// <param name="fileContentStream"></param>
        /// <returns></returns>
        private async Task<Tuple<File, FilePackage>> GetNewFileModelAsync(string fileName, System.IO.Stream fileContentStream)
        {
            var newFile = new File()
            {
                id = Guid.NewGuid(),
                name_text = fileName,
                date_of_change = DateTime.Now,
                strings_count = 0,
                version = this._initialFileVersion,
                priority = 0,
                is_folder = false,
                is_last_version = true,
                visibility = true,
            };

            var fileExtension = fileName.Split(".", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            string fileContent = string.Empty;
            string fileEncoding = string.Empty;
            FilePackage filePackage = null;
            if (this.FileExtensionToContentFileName.ContainsKey(fileExtension))
            {
                var filePackageFolderName = this.GetUniqueFileSystemName();
                System.IO.Directory.CreateDirectory(filePackageFolderName);
                var filePackageName = System.IO.Path.Combine(filePackageFolderName, newFile.name_text);
                using (fileContentStream)
                using (var filePackageStream = System.IO.File.Create(filePackageName))
                {
                    var fileContentStreamLength = (int)fileContentStream.Length;
                    var temp = new byte[fileContentStreamLength];
                    await fileContentStream.ReadAsync(
                        buffer: temp,
                        offset: 0,
                        count: fileContentStreamLength);
                    filePackage = new FilePackage()
                    {
                        file_id = newFile.id,
                        data = temp,
                        content_file_name = this.FileExtensionToContentFileName[fileExtension]
                    };
                    await filePackageStream.WriteAsync(temp);
                }
                var filePackageUnpackedFolderName = this.GetUniqueFileSystemName();
                ZipFile.ExtractToDirectory(
                    sourceArchiveFileName: filePackageName,
                    destinationDirectoryName: filePackageUnpackedFolderName);

                var contentFileName = System.IO.Path.Combine(filePackageUnpackedFolderName, filePackage.content_file_name);

                using (var fileStream = System.IO.File.OpenRead(contentFileName))
                using (var fileContentStreamReader = new System.IO.StreamReader(fileStream))
                {
                    fileContent = fileContentStreamReader.ReadToEnd();
                    fileEncoding = fileContentStreamReader.CurrentEncoding.WebName;
                }
                System.IO.Directory.Delete(filePackageUnpackedFolderName, recursive: true);
            }
            else
            {
                using (fileContentStream)
                using (var fileContentStreamReader = new System.IO.StreamReader(fileContentStream))
                {
                    fileContent = fileContentStreamReader.ReadToEnd();
                    fileEncoding = fileContentStreamReader.CurrentEncoding.WebName;
                }
            }
            newFile.original_full_text = fileContent;
            newFile.encod = fileEncoding;

            return Tuple.Create(newFile, filePackage);
        }

        private File GetNewFolderModel()
        {
            return new File()
            {
                date_of_change = DateTime.Now,
                is_folder = true,
                is_last_version = true,
                visibility = true
            };
        }

        private File GetNewFolderModel(string folderName, Guid? folderOwnerId, Guid localizationProjectId)
        {
            var newFolder = this.GetNewFolderModel();
            newFolder.name_text = folderName;
            newFolder.id_folder_owner = folderOwnerId;
            newFolder.id_localization_project = localizationProjectId;
            return newFolder;
        }

        public async Task<Node<File>> AddFolderAsync(FolderModel newFolderModel, Guid projectId)
        {
            var foundedFolder = await this._filesRepository.GetLastVersionByNameAndParentIdAsync(newFolderModel.name, newFolderModel.parentId, projectId);
            if (foundedFolder != null)
            {
                throw new Exception(WriteLn($"Папка \"{newFolderModel.name}\" уже есть."));
            }

            var newFolder = this.GetNewFolderModel(
                folderName: newFolderModel.name,
                folderOwnerId: newFolderModel.parentId,
                localizationProjectId: newFolderModel.projectId
                );
            return await AddNodeAsync(newFolder, insertToDbAction: this.InsertFolderToDbAsync, projectId: projectId);
        }

        public async Task AddFolderWithContentsAsync(IFormFileCollection files, Guid? parentId, Guid projectId, string signalrClientId)
        {
            foreach (var file in files)
            {
                var relativePathToFile = file.FileName;
                var directoriesToFile =
                    System.IO.Path.GetDirectoryName(relativePathToFile)
                        .Split(System.IO.Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
                var lastParentId = parentId;
                foreach (var directoryName in directoriesToFile)
                {
                    var directoryDbModel = await this._filesRepository.GetLastVersionByNameAndParentIdAsync(directoryName, lastParentId, projectId);
                    if (directoryDbModel == null)
                    {
                        var newFolder = this.GetNewFolderModel(
                            folderName: directoryName,
                            folderOwnerId: lastParentId,
                            localizationProjectId: projectId
                            );
                        lastParentId = await this._filesRepository.AddAsync(newFolder);
                    }
                    else
                    {
                        lastParentId = directoryDbModel.id;
                    }
                }

                var fileName = System.IO.Path.GetFileName(relativePathToFile);
                using (var fileContentStream = file.OpenReadStream())
                {
                    var (newFile, filePackage) = await this.GetNewFileModelAsync(
                        fileName: fileName,
                        fileContentStream: fileContentStream);
                    newFile.id_folder_owner = lastParentId;
                    newFile.id_localization_project = projectId;

                    try
                    {
                        await this.InsertFileToDbAsync(newFile, filePackage);
                    }
                    catch (Parser.ParserException parserException)
                    {
                        await this.FileParsingFailed?.Invoke(
                            signalrClientId,
                            new FailedFileParsingModel()
                            {
                                FileName = relativePathToFile,
                                ParserMessage = parserException.Message,
                            });
                    }
                }
            }
        }

        public async Task UpdateNodeAsync(Guid id, File file)
        {
            // Check if file by id exists in database
            var foundedFile = await this._filesRepository.GetByIDAsync(id);
            if (foundedFile == null)
            {
                throw new Exception(WriteLn($"Не найдено файла/папки с id \"{id}\"."));
            }

            file.id = id;
            file.version = foundedFile.version;
            file.is_last_version = foundedFile.is_last_version;
            file.date_of_change = DateTime.Now;

            var filePackage = await this._filesPackagesRepository.GetByFileIdAsync(fileId: foundedFile.id);
            if (filePackage != null)
            {
                filePackage.file_id = file.id;
                var filePackageCopied = await this._filesPackagesRepository.AddAsync(filePackage);
                if (!filePackageCopied)
                {
                    throw new Exception(WriteLn($"Не удалось обновить файл \"{foundedFile.name_text}\"."));
                }
            }

            var updatedSuccessfully = await this._filesRepository.UpdateAsync(file);
            if (!updatedSuccessfully)
            {
                throw new Exception(WriteLn($"Не удалось обновить файл \"{foundedFile.name_text}\"."));
            }
        }

        public async Task DeleteNodeAsync(File file)
        {
            var id = file.id;
            var foundedFile = await this._filesRepository.GetByIDAsync(id);
            if (foundedFile == null)
            {
                throw new Exception(WriteLn($"Не найдено файла/папки с id \"{id}\"."));
            }

            var glossary = await this._glossaryRepository.GetByFileIdAsync(id);
            if (glossary != null)
            {
                throw new Exception(WriteLn("Удаление файла словаря запрещено."));
            }

            var tempFileModel = foundedFile;
            do
            {
                await this._filesRepository.RemoveAsync(id: tempFileModel.id);
                if (tempFileModel.id_previous_version.HasValue)
                {
                    tempFileModel = await this._filesRepository.GetByIDAsync(tempFileModel.id_previous_version.Value);
                }
                else
                {
                    break;
                }
            } while (tempFileModel != null);
        }

        private async Task<Node<File>> AddNodeAsync(File file, Func<File, Task> insertToDbAction, Guid projectId)
        {
            if (file.id_folder_owner.HasValue)
            {
                var parentFile = await this._filesRepository.GetByIDAsync(file.id_folder_owner.Value);
                if (parentFile?.is_folder == false)
                {
                    throw new Exception(WriteLn($"Нельзя добавить файл/папку \"{file.name_text}\", т.к. нельзя иметь файл в качестве родителя."));
                }
            }

            await insertToDbAction(file);

            var addedFile = await this._filesRepository.GetLastVersionByNameAndParentIdAsync(file.name_text, file.id_folder_owner, projectId);
            var icon = GetIconByFile(addedFile);
            return new Node<File>(addedFile, icon);
        }

        private async Task InsertFileToDbAsync(File file, FilePackage filePackage)
        {
            var projectLocales = await this._localeRepository.GetAllForProject(projectId: (Guid)file.id_localization_project);

            var fileUploaded = await this._filesRepository.UploadAsync(file, projectLocales);
            if (!fileUploaded)
            {
                Exception e = new Exception(($"Не удалось добавить файл \"{file.name_text}\" в базу данных."));
                WriteLn($"Не удалось добавить файл \"{file.name_text}\" в базу данных.", e);
                throw e;
            }

            if (filePackage != null)
            {
                var filePackageUploaded = await this._filesPackagesRepository.AddAsync(filePackage);
            }

            var addedFileId = (await this._filesRepository.GetLastVersionByNameAndParentIdAsync(file.name_text, file.id_folder_owner, (Guid)file.id_localization_project)).id;

            await this._filesRepository.AddTranslationLocalesAsync(
                fileId: addedFileId,
                localesIds: projectLocales.Select(locale => locale.id));
        }

        private async Task InsertFolderToDbAsync(File file)
        {
            try
            {
                file.visibility = true;
                await this._filesRepository.AddAsync(file);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Не удалось добавить папку \"{file.name_text}\" в базу данных.", exception), exception);
            }
        }

        private string GetIconByFile(File file)
        {
            var pathPrefix = "assets/svg/";
            if (file.is_folder)
            {
                return $"{pathPrefix}067-folder.svg";
            }

            return $"{pathPrefix}022-write.svg";
        }

        public async Task ChangeParentFolderAsync(Guid fileId, Guid? newParentId)
        {
            var foundedFile = await this._filesRepository.GetByIDAsync(fileId);
            if (foundedFile == null)
            {
                throw new Exception(WriteLn($"Не найдено файла/папки с id \"{fileId}\"."));
            }

            if (newParentId.HasValue)
            {
                var newParent = await this._filesRepository.GetByIDAsync(id: newParentId.Value);
                if (newParent == null)
                {
                    throw new Exception(WriteLn("Указанной родительской папки не существует."));
                }

                if (!newParent.is_folder)
                {
                    throw new Exception(WriteLn("Указанный родитель не является папкой."));
                }

                if (fileId == newParentId.Value)
                {
                    throw new Exception(WriteLn("Папка не может быть родительской по отношению к себе."));
                }
            }

            await this._filesRepository.ChangeParentFolderAsync(
                fileId: fileId,
                newParentId: newParentId
                );
        }

        public async Task<IEnumerable<Locale>> GetTranslationLocalesForFileAsync(Guid fileId)
        {
            return await this._filesRepository.GetLocalesForFileAsync(fileId: fileId);
        }

        public async Task UpdateTranslationLocalesForFileAsync(Guid fileId, IEnumerable<Guid> localesIds)
        {
            //TODO переделать под одну транзакуцию
            await this._filesRepository.DeleteTranslationLocalesAsync(fileId: fileId);
            await this._filesRepository.AddTranslationLocalesAsync(fileId: fileId, localesIds: localesIds);
        }

        public async Task<System.IO.FileStream> GetFileAsync(Guid fileId, Guid? localeId)
        {
            var file = await this._filesRepository.GetByIDAsync(id: fileId);
            if (file == null)
            {
                throw new Exception(WriteLn("Файл не найден.  " + fileId));
            }

            if (file.is_folder)
            {

                var uniqueTempFolderPath = this.GetUniqueFileSystemName();
                var currentLevelFiles = new Dictionary<File, string>() { { file, uniqueTempFolderPath } };
                var compressedFileName = this.GetUniqueFileSystemName();
                try
                {
                    while (currentLevelFiles.Any())
                    {
                        var newLevelFiles = new Dictionary<File, string>();
                        foreach (var fileToPath in currentLevelFiles)
                        {
                            var currentLevelFile = fileToPath.Key;
                            var currentLevelPath = fileToPath.Value;
                            var fileName = string
                                .IsNullOrWhiteSpace(currentLevelFile.download_name)
                                ? currentLevelFile.name_text
                                : currentLevelFile.download_name;
                            if (currentLevelFile.is_folder)
                            {
                                System.IO.Directory.CreateDirectory(currentLevelPath);
                                var newFolderPath = System.IO.Path.Combine(currentLevelPath, fileName);
                                System.IO.Directory.CreateDirectory(newFolderPath);
                                var children = await this._filesRepository
                                    .GetFilesByParentFolderIdAsync(parentFolderId: currentLevelFile.id);
                                foreach (var child in children)
                                {
                                    newLevelFiles[child] = newFolderPath;
                                }
                            }
                            else
                            {
                                System.IO.Directory.CreateDirectory(currentLevelPath);
                                var filePath = System.IO.Path.Combine(currentLevelPath, fileName);
                                var fileStream = await this.WriteFileAsync(
                                    file: currentLevelFile,
                                    filePath: filePath,
                                    localeId: localeId);
                                fileStream.Dispose();
                            }
                        }

                        currentLevelFiles = newLevelFiles;
                    }



                    var compressionLevel = Settings.getSettings().GetString("zip_CompressionLevel");

                    CompressionLevel level = CompressionLevel.Fastest;

                    if (compressionLevel.Equals("Optimal"))
                    {
                        level = CompressionLevel.Optimal;
                    }
                    else if (compressionLevel.Equals("NoCompression"))
                    {
                        level = CompressionLevel.NoCompression;
                    }


                    ZipFile.CreateFromDirectory(
                        sourceDirectoryName: System.IO.Path.Combine(uniqueTempFolderPath, file.name_text),
                        destinationArchiveFileName: compressedFileName, compressionLevel: level,
                        includeBaseDirectory: true);
                }
                finally
                {
                    System.IO.Directory.Delete(uniqueTempFolderPath, recursive: true);
                }

                return System.IO.File.OpenRead(compressedFileName);
            }
            else
            {
                var tempFileName = System.IO.Path.GetTempFileName();
                return await this.WriteFileAsync(
                    file: file,
                    filePath: tempFileName,
                    localeId: localeId);
            }
        }

        public async Task<System.IO.FileStream> WriteFileAsync(File file, string filePath, Guid? localeId)
        {
            var filePackage = await this._filesPackagesRepository.GetByFileIdAsync(fileId: file.id);
            if (filePackage != null)
            {
                var folderPackageName = this.GetUniqueFileSystemName();
                System.IO.Directory.CreateDirectory(folderPackageName);
                var filePackageName = System.IO.Path.Combine(folderPackageName, file.name_text);
                var filePackageStream = System.IO.File.Create(filePackageName);
                try
                {
                    await filePackageStream.WriteAsync(filePackage.data);
                }
                catch (Exception)
                {
                    filePackageStream.Dispose();
                    throw;
                }

                if (localeId.HasValue)
                {
                    filePackageStream.Dispose();
                    var filePackageUnpackedFolderName = this.GetUniqueFileSystemName();
                    ZipFile.ExtractToDirectory(
                        sourceArchiveFileName: filePackageName,
                        destinationDirectoryName: filePackageUnpackedFolderName);

                    var contentFileName = System.IO.Path.Combine(filePackageUnpackedFolderName, filePackage.content_file_name);
                    var fileContent = await this._filesRepository
                        .GetFileContentAsync(
                            id: file.id,
                            id_locale: localeId.Value);

                    using (var fileStream = System.IO.File.Create(contentFileName))
                    using (var streamWriter = new System.IO.StreamWriter(
                        stream: fileStream,
                        encoding: Encoding.GetEncoding(file.encod),
                        bufferSize: this._defaultFileStreamBufferSize,
                        leaveOpen: false))
                    {
                        await streamWriter.WriteAsync(fileContent);
                    }
                    ZipFile.CreateFromDirectory(
                        sourceDirectoryName: filePackageUnpackedFolderName,
                        destinationArchiveFileName: filePackageName);
                    System.IO.Directory.Delete(filePackageUnpackedFolderName, recursive: true);
                    return System.IO.File.OpenRead(filePackageName);
                }
                else
                {
                    try
                    {
                        filePackageStream.Seek(0, System.IO.SeekOrigin.Begin);
                    }
                    catch (Exception)
                    {
                        filePackageStream.Dispose();
                        throw;
                    }
                    return filePackageStream;
                }
            }
            else
            {
                var fileContent = await this._filesRepository
                    .GetFileContentAsync(
                        id: file.id,
                        id_locale: localeId);

                var fileStream = System.IO.File.Create(filePath);
                try
                {
                    using (var streamWriter = new System.IO.StreamWriter(
                        stream: fileStream,
                        encoding: Encoding.GetEncoding(file.encod),
                        bufferSize: this._defaultFileStreamBufferSize,
                        leaveOpen: true))
                    {
                        await streamWriter.WriteAsync(fileContent);
                    }
                    fileStream.Seek(0, System.IO.SeekOrigin.Begin);
                }
                catch (Exception)
                {
                    fileStream.Dispose();
                    throw;
                }
                return fileStream;
            }
        }

        private string GetUniqueFileSystemName()
        {
            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
        }

    }
}
