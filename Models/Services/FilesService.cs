using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models.DatabaseEntities;
using Models.Extensions;
using Models.Interfaces.Repository;
using Models.Models;

namespace Models.Services
{
    public class FilesService : BaseService
    {
        private readonly int _initialFileVersion = 1;
        private readonly int _defaultFileStreamBufferSize = 4096;
        private readonly IFilesRepository _filesRepository;
        private readonly IGlossaryRepository _glossaryRepository;
        private readonly ILocaleRepository _localeRepository;

        public FilesService(
            IFilesRepository filesRepository,
            IGlossaryRepository glossaryRepository,
            ILocaleRepository localeRepository
            )
        {
            this._filesRepository = filesRepository;
            this._glossaryRepository = glossaryRepository;
            this._localeRepository = localeRepository;
        }

        public async Task<IEnumerable<Node<File>>> GetAll()
        {
            var files = await this._filesRepository.GetAllAsync();
            return files?.ToTree((file, icon) => new Node<File>(file, icon), (file) => GetIconByFile(file));
        }

        public async Task<IEnumerable<Node<File>>> GetByProjectIdAsync(int projectId, string fileNamesSearch)
        {
            if (string.IsNullOrEmpty(fileNamesSearch))
            {
                var files = await this._filesRepository.GetByProjectIdAsync(projectId: projectId);
                return files.ToTree((file, icon) => new Node<File>(file, icon), (file) => this.GetIconByFile(file));
            }
            else
            {
                var idsToFiles = (await this._filesRepository.GetByProjectIdAsync(projectId: projectId, fileNamesSearch: fileNamesSearch))
                    .ToDictionary(keySelector: value => value.ID);
                var parentsIds = idsToFiles.Where(x => x.Value.ID_FolderOwner != null
                                                    && !idsToFiles.ContainsKey(x.Value.ID_FolderOwner.Value))
                                           .Select(x => (int)x.Value.ID_FolderOwner)
                                           .ToList();
                do
                {
                    var newParentsIds = new List<int>();
                    foreach (var parentId in parentsIds)
                    {
                        var parentFile = await this._filesRepository.GetByIDAsync(parentId);
                        idsToFiles[parentFile.ID] = parentFile;
                        if (parentFile.ID_FolderOwner != null
                            && !idsToFiles.ContainsKey(parentFile.ID_FolderOwner.Value))
                        {
                            newParentsIds.Add(parentFile.ID_FolderOwner.Value);
                        }
                    }
                    parentsIds = newParentsIds;
                } while (parentsIds.Any());
                return idsToFiles.Values.ToTree((file, icon) => new Node<File>(file, icon), (file) => this.GetIconByFile(file));
            }
        }

        public async Task<File> GetById(int id)
        {
            return await this._filesRepository.GetByIDAsync(id);
        }

        public IEnumerable<File> GetInitialFolders(int projectId)
        {
            return this._filesRepository.GetInitialFolders(projectId);
        }

        public async Task<Node<File>> AddFile(string fileName, System.IO.Stream fileContentStream, int? parentId, int projectId)
        {
            var foundedFile = await this._filesRepository.GetLastVersionByNameAndParentId(fileName, parentId);
            if (foundedFile != null)
            {
                throw new Exception(WriteLn($"Файл \"{fileName}\" уже есть."));
            }

            var newFile = this.GetNewFileModel(fileContentStream);
            newFile.Name = fileName;
            newFile.ID_FolderOwner = parentId;
            newFile.ID_LocalizationProject = projectId;

            return await this.AddNode(newFile, insertToDbAction: this.InsertFileToDbAsync);
        }

        public async Task<Node<File>> UpdateFileVersion(string fileName, System.IO.Stream fileContentStream, int? parentId, int projectId)
        {
            var version = this._initialFileVersion;
            var lastVersionDbFile = await this._filesRepository.GetLastVersionByNameAndParentId(fileName, parentId);
            if (lastVersionDbFile != null)
            {
                if (lastVersionDbFile.IsFolder)
                {
                    throw new Exception(WriteLn("Нельзя обновить папку."));
                }

                lastVersionDbFile.IsLastVersion = false;
                if (!lastVersionDbFile.Version.HasValue)
                {
                    lastVersionDbFile.Version = this._initialFileVersion;
                }
                version = lastVersionDbFile.Version.Value + 1;

                // TODO: single transaction?
                var updatedSuccessfully = await this._filesRepository.UpdateAsync(lastVersionDbFile);
                if (!updatedSuccessfully)
                {
                    throw new Exception(WriteLn("Не удалось обновить старый файл."));
                }
            }

            var newVersionFile = this.GetNewFileModel(fileContentStream);
            newVersionFile.Name = fileName;
            newVersionFile.ID_FolderOwner = parentId;
            newVersionFile.ID_LocalizationProject = projectId;
            newVersionFile.Version = version;
            newVersionFile.Id_PreviousVersion = lastVersionDbFile?.ID;

            var newNode = await this.AddNode(newVersionFile, insertToDbAction: this.InsertFileToDbAsync);

            if (lastVersionDbFile != null)
            {
                var localesIds = (await this._filesRepository.GetLocalesForFileAsync(fileId: lastVersionDbFile.ID))
                                 .Select(x => x.ID);
                await this.UpdateTranslationLocalesForTermAsync(fileId: newNode.Data.ID, localesIds: localesIds);
            }
            return newNode;
        }

        public async Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoAsync(int fileId)
        {
            var file = await this._filesRepository.GetByIDAsync(id: fileId);
            if (file.IsFolder)
            {
                var folderTranslationInfos = new List<FileTranslationInfo>();
                var childTranslationInfos = await this.GetFileTranslationInfoRecursiveAsync(file: file);
                var groupedByLocale = childTranslationInfos.GroupBy(x => x.LocaleId);
                foreach (var localeIdToTranslationInfos in groupedByLocale)
                {
                    var localeId = localeIdToTranslationInfos.Key;
                    var translationInfos = localeIdToTranslationInfos.ToArray();
                    var filesCount = translationInfos.Length;
                    var averageConfirmed = translationInfos
                        .Aggregate(seed: 0d, func: (seed, translationInfo) => seed + translationInfo.PercentOfConfirmed) / filesCount;
                    var averageTranslated = translationInfos
                        .Aggregate(seed: 0d, func: (seed, translationInfo) => seed + translationInfo.PercentOfTranslation) / filesCount;
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
                return await this._filesRepository.GetFileTranslationInfoByIdAsync(fileId: file.ID);
            }
        }

        private async Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoRecursiveAsync(File file)
        {
            if (file.IsFolder)
            {
                var translationIdsToInfos = new List<FileTranslationInfo>();
                var children = await this._filesRepository.GetFilesByParentFolderIdAsync(parentFolderId: file.ID);
                foreach (var childFile in children)
                {
                    var childTranslationInfos = await this.GetFileTranslationInfoRecursiveAsync(childFile);
                    translationIdsToInfos.AddRange(childTranslationInfos);
                }
                return translationIdsToInfos;
            }
            else
            {
                return await this._filesRepository.GetFileTranslationInfoByIdAsync(fileId: file.ID);
            }
        }

        private File GetNewFileModel(System.IO.Stream fileContentStream)
        {
            var newFile = new File()
            {
                DateOfChange = DateTime.Now,
                StringsCount = 0,
                Version = this._initialFileVersion,
                Priority = 0,
                IsFolder = false,
                IsLastVersion = true,
            };

            string fileContent = string.Empty;
            string fileEncoding = string.Empty;
            using (fileContentStream)
            using (var fileContentStreamReader = new System.IO.StreamReader(fileContentStream))
            {
                fileContent = fileContentStreamReader.ReadToEnd();
                fileEncoding = fileContentStreamReader.CurrentEncoding.WebName;
            }
            newFile.OriginalFullText = fileContent;
            newFile.Encoding = fileEncoding;

            return newFile;
        }

        private File GetNewFolderModel()
        {
            return new File()
            {
                DateOfChange = DateTime.Now,
                IsFolder = true,
                IsLastVersion = true,
            };
        }

        private File GetNewFolderModel(string folderName, int? folderOwnerId, int localizationProjectId)
        {
            var newFolder = this.GetNewFolderModel();
            newFolder.Name = folderName;
            newFolder.ID_FolderOwner = folderOwnerId;
            newFolder.ID_LocalizationProject = localizationProjectId;
            return newFolder;
        }

        public async Task<Node<File>> AddFolder(FolderModel newFolderModel)
        {
            var foundedFolder = await this._filesRepository.GetLastVersionByNameAndParentId(newFolderModel.Name, newFolderModel.ParentId);
            if (foundedFolder != null)
            {
                throw new Exception(WriteLn($"Папка \"{newFolderModel.Name}\" уже есть."));
            }

            var newFolder = this.GetNewFolderModel(
                folderName: newFolderModel.Name,
                folderOwnerId: newFolderModel.ParentId,
                localizationProjectId: newFolderModel.ProjectId
                );
            return await AddNode(newFolder, insertToDbAction: this.InsertFolderToDbAsync);
        }

        public async Task AddFolderWithContents(IFormFileCollection files, int? parentId, int projectId)
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
                    var directoryDbModel = await this._filesRepository.GetLastVersionByNameAndParentId(directoryName, lastParentId);
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
                        lastParentId = directoryDbModel.ID;
                    }
                }

                var fileName = System.IO.Path.GetFileName(relativePathToFile);

                File newFile = null;
                using (var fileContentStream = file.OpenReadStream())
                {
                    newFile = this.GetNewFileModel(fileContentStream);
                }
                newFile.Name = fileName;
                newFile.ID_FolderOwner = lastParentId;
                newFile.ID_LocalizationProject = projectId;
                await this.InsertFileToDbAsync(newFile);
            }
        }

        public async Task UpdateNode(int id, File file)
        {
            // Check if file by id exists in database
            var foundedFile = await this._filesRepository.GetByIDAsync(id);
            if (foundedFile == null)
            {
                throw new Exception(WriteLn($"Не найдено файла/папки с id \"{id}\"."));
            }

            file.ID = id;
            file.Version = foundedFile.Version;
            file.IsLastVersion = foundedFile.IsLastVersion;
            file.DateOfChange = DateTime.Now;
            var updatedSuccessfully = await this._filesRepository.UpdateAsync(file);
            if (!updatedSuccessfully)
            {
                throw new Exception(WriteLn($"Не удалось обновить файл \"{foundedFile.Name}\"."));
            }
        }

        public async Task DeleteNode(File file)
        {
            var id = file.ID;
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
                await this._filesRepository.RemoveAsync(id: tempFileModel.ID);
                if (tempFileModel.Id_PreviousVersion.HasValue)
                {
                    tempFileModel = await this._filesRepository.GetByIDAsync(tempFileModel.Id_PreviousVersion.Value);
                }
                else
                {
                    break;
                }
            } while (tempFileModel != null);
        }

        private async Task<Node<File>> AddNode(File file, Func<File, Task> insertToDbAction)
        {
            if (file.ID_FolderOwner.HasValue)
            {
                var parentFile = await this._filesRepository.GetByIDAsync(file.ID_FolderOwner.Value);
                if (parentFile?.IsFolder == false)
                {
                    throw new Exception(WriteLn($"Нельзя добавить файл/папку \"{file.Name}\", т.к. нельзя иметь файл в качестве родителя."));
                }
            }

            await insertToDbAction(file);

            var addedFile = await this._filesRepository.GetLastVersionByNameAndParentId(file.Name, file.ID_FolderOwner);
            var icon = GetIconByFile(addedFile);
            return new Node<File>(addedFile, icon);
        }

        private async Task InsertFileToDbAsync(File file)
        {
            var fileUploaded = await this._filesRepository.Upload(file);
            if (!fileUploaded)
            {
                throw new Exception(WriteLn($"Не удалось добавить файл \"{file.Name}\" в базу данных."));
            }

            var addedFileId = (await this._filesRepository.GetLastVersionByNameAndParentId(file.Name, file.ID_FolderOwner)).ID;
            var projectLocales = await this._localeRepository.GetAllForProject(projectId: file.ID_LocalizationProject);
            await this._filesRepository.AddTranslationLocalesAsync(
                fileId: addedFileId,
                localesIds: projectLocales.Select(locale => locale.ID));
        }

        private async Task InsertFolderToDbAsync(File file)
        {
            try
            {
                await this._filesRepository.AddAsync(file);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Не удалось добавить папку \"{file.Name}\" в базу данных.", exception), exception);
            }
        }

        private string GetIconByFile(File file)
        {
            var pathPrefix = "assets/fileIcons/";
            if (file.IsFolder)
            {
                return $"{pathPrefix}folder.png";
            }

            return $"{pathPrefix}defaultFile.png";
        }

        public async Task ChangeParentFolderAsync(int fileId, int? newParentId)
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

                if (!newParent.IsFolder)
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

        public async Task<IEnumerable<Locale>> GetTranslationLocalesForFileAsync(int fileId)
        {
            return await this._filesRepository.GetLocalesForFileAsync(fileId: fileId);
        }

        public async Task UpdateTranslationLocalesForTermAsync(int fileId, IEnumerable<int> localesIds)
        {
            await this._filesRepository.DeleteTranslationLocalesAsync(fileId: fileId);
            await this._filesRepository.AddTranslationLocalesAsync(fileId: fileId, localesIds: localesIds);
        }

        public async Task<System.IO.FileStream> GetFile(int fileId, int? localeId)
        {
            var file = await this._filesRepository.GetByIDAsync(id: fileId);
            if (file == null)
            {
                throw new Exception("Файл не найден.");
            }

            var fileContent = await this._filesRepository
                .GetFileContent(
                    id: fileId,
                    id_locale: localeId.HasValue ? localeId.Value : -1);
            var tempFileName = System.IO.Path.GetTempFileName();
            var fileStream = System.IO.File.Create(tempFileName);
            using (var sw = new System.IO.StreamWriter(
                stream: fileStream,
                encoding: Encoding.GetEncoding(file.Encoding),
                bufferSize: this._defaultFileStreamBufferSize,
                leaveOpen: true))
            {
                sw.Write(fileContent);
            }
            fileStream.Seek(0, System.IO.SeekOrigin.Begin);
            return fileStream;
        }

    }
}
