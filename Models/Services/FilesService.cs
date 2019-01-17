﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.Extensions;
using Models.Interfaces.Repository;
using Models.Models;

namespace Models.Services
{
    public class FilesService
    {

        private readonly IFilesRepository _filesRepository;

        private readonly IGlossaryRepository _glossaryRepository;

        public FilesService(
            IFilesRepository filesRepository,
            IGlossaryRepository glossaryRepository
            )
        {
            this._filesRepository = filesRepository;
            this._glossaryRepository = glossaryRepository;
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

            string fileContent = string.Empty;
            using (fileContentStream)
            using (var fileContentStreamReader = new System.IO.StreamReader(fileContentStream))
            {
                fileContent = fileContentStreamReader.ReadToEnd();
            }

            var newFile = this.GetNewFileModel();
            newFile.Name = fileName;
            newFile.OriginalFullText = fileContent;
            newFile.ID_FolderOwner = parentId;
            newFile.ID_LocalizationProject = projectId;

            return await this.AddNode(newFile, insertToDbAction: this.InsertFileToDbAsync);
        }

        public async Task<Node<File>> UpdateFileVersion(string fileName, System.IO.Stream fileContentStream, int? parentId, int projectId)
        {
            var version = 0;
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
                    lastVersionDbFile.Version = 0;
                }
                version = lastVersionDbFile.Version.Value + 1;

                // TODO: single transaction?
                var updatedSuccessfully = await this._filesRepository.UpdateAsync(lastVersionDbFile);
                if (!updatedSuccessfully)
                {
                    throw new Exception("Не удалось обновить старый файл.");
                }
            }

            string fileContent = string.Empty;
            using (fileContentStream)
            using (var fileContentStreamReader = new System.IO.StreamReader(fileContentStream))
            {
                fileContent = fileContentStreamReader.ReadToEnd();
            }

            var newVersionFile = this.GetNewFileModel();
            newVersionFile.Name = fileName;
            newVersionFile.OriginalFullText = fileContent;
            newVersionFile.ID_FolderOwner = parentId;
            newVersionFile.ID_LocalizationProject = projectId;
            newVersionFile.Version = version;

            return await this.AddNode(newVersionFile, insertToDbAction: this.InsertFileToDbAsync);
        }

        private File GetNewFileModel()
        {
            return new File()
            {
                DateOfChange = DateTime.Now,
                StringsCount = 0,
                Version = 0,
                Priority = 0,
                IsFolder = false,
                //TODO: file encoding
                Encoding = "",
                IsLastVersion = true,
            };
        }

        public async Task<Node<File>> AddFolder(FolderModel newFolderModel)
        {
            var foundedFolder = await this._filesRepository.GetLastVersionByNameAndParentId(newFolderModel.Name, newFolderModel.ParentId);
            if (foundedFolder != null)
            {
                throw new Exception($"Папка \"{newFolderModel.Name}\" уже есть.");
            }

            var newFolder = new File
            {
                Name = newFolderModel.Name,
                IsFolder = true,
                ID_FolderOwner = newFolderModel.ParentId,
                ID_LocalizationProject = newFolderModel.ProjectId
            };
            return await AddNode(newFolder, insertToDbAction: this.InsertFolderToDbAsync);
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

        public async Task DeleteNode(int id)
        {
            // Check if file by id exists in database
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

            var deleteSuccessfully = await this._filesRepository.RemoveAsync(id);
            if (!deleteSuccessfully)
            {
                throw new Exception($"Не удалось удалить файл, имеющий id \"{id}\".");
            }
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
        }

        private async Task InsertFolderToDbAsync(File file)
        {
            var folderAdded = await this._filesRepository.AddAsync(file) > 0;
            if (!folderAdded)
            {
                throw new Exception($"Не удалось добавить папку \"{file.Name}\" в базу данных.");
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

    }
}
