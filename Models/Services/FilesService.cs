using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models.DatabaseEntities;
using Models.Extensions;
using Models.Interfaces.Repository;
using Models.Models;

namespace Models.Services
{
    public class FilesService
    {
        private readonly int _initialFileVersion = 1;
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
            IEnumerable<File> files;
            if (string.IsNullOrEmpty(fileNamesSearch))
            {
                files = await this._filesRepository.GetByProjectIdAsync(projectId: projectId);
                return files?.ToTree((file, icon) => new Node<File>(file, icon), (file) => this.GetIconByFile(file));
            }
            else
            {
                files = await this._filesRepository.GetByProjectIdAsync(projectId: projectId, fileNamesSearch: fileNamesSearch);
                return files?.Select(file => new Node<File>(file, this.GetIconByFile(file)));
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
                throw new Exception($"Файл \"{fileName}\" уже есть.");
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
                    throw new Exception("Нельзя обновить папку.");
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
                    throw new Exception("Не удалось обновить старый файл.");
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
                throw new Exception($"Папка \"{newFolderModel.Name}\" уже есть.");
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
                throw new Exception($"Не найдено файла/папки с id \"{id}\".");
            }

            file.ID = id;
            file.Version = foundedFile.Version;
            file.IsLastVersion = foundedFile.IsLastVersion;
            file.DateOfChange = DateTime.Now;
            var updatedSuccessfully = await this._filesRepository.UpdateAsync(file);
            if (!updatedSuccessfully)
            {
                throw new Exception($"Не удалось обновить файл \"{foundedFile.Name}\".");
            }
        }

        public async Task DeleteNode(File file)
        {
            var id = file.ID;
            var foundedFile = await this._filesRepository.GetByIDAsync(id);
            if (foundedFile == null)
            {
                throw new Exception($"Не найдено файла/папки с id \"{id}\".");
            }

            var glossary = await this._glossaryRepository.GetByFileIdAsync(id);
            if (glossary != null)
            {
                throw new Exception("Удаление файла словаря запрещено.");
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
                    throw new Exception($"Нельзя добавить файл/папку \"{file.Name}\", т.к. нельзя иметь файл в качестве родителя.");
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
                throw new Exception($"Не удалось добавить файл \"{file.Name}\" в базу данных.");
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
                throw new Exception($"Не удалось добавить папку \"{file.Name}\" в базу данных.", exception);
            }
        }

        private string GetIconByFile(File file)
        {
            // Icons section in JSON configuration
            //var iconsSection = configuration.GetSection("icons");

            //// If node is folder, return folder icon 
            //if (file.IsFolder)
            //{
            //    // var iconBundle = iconsSection.GetSection("folder").Get<IconBundle>();

            //    return iconsSection.GetValue<string>("folder");
            //}

            //// Get file extension by file name
            //var extension = System.IO.Path.GetExtension(file.Name);

            //// Get font-awesome icon class by file extension
            //return iconsSection.GetValue(extension, "assets/icons/file.png");

            return null;
        }

        public async Task ChangeParentFolderAsync(int fileId, int? newParentId)
        {
            var foundedFile = await this._filesRepository.GetByIDAsync(fileId);
            if (foundedFile == null)
            {
                throw new Exception($"Не найдено файла/папки с id \"{fileId}\".");
            }

            if (newParentId.HasValue)
            {
                var newParent = await this._filesRepository.GetByIDAsync(id: newParentId.Value);
                if (newParent == null)
                {
                    throw new Exception("Указанной родительской папки не существует.");
                }

                if (!newParent.IsFolder)
                {
                    throw new Exception("Указанный родитель не является папкой.");
                }

                if (fileId == newParentId.Value)
                {
                    throw new Exception("Папка не может быть родительской по отношению к себе.");
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

    }
}
