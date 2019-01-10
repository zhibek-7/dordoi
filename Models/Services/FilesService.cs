using System;
using System.Collections.Generic;
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

        public FilesService(IFilesRepository filesRepository)
        {
            this._filesRepository = filesRepository;
        }

        public async Task<IEnumerable<Node<File>>> GetAll()
        {
            var files = await this._filesRepository.GetAllAsync();
            return files?.ToTree((file, icon) => new Node<File>(file, icon), (file) => GetIconByFile(file));
        }

        public async Task<IEnumerable<Node<File>>> GetByProjectIdAsync(int projectId)
        {
            var files = await this._filesRepository.GetByProjectIdAsync(projectId: projectId);
            return files?.ToTree((file, icon) => new Node<File>(file, icon), (file) => GetIconByFile(file));
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
            var foundedFile = await this._filesRepository.GetByNameAndParentId(fileName, parentId);
            if (foundedFile != null)
            {
                throw new Exception($"File \"{fileName}\" already exists");
            }

            string fileContent = string.Empty;
            using (fileContentStream)
            using (var fileContentStreamReader = new System.IO.StreamReader(fileContentStream))
            {
                fileContent = fileContentStreamReader.ReadToEnd();
            }

            var newFile = new File
            {
                Name = fileName,
                DateOfChange = DateTime.Now,
                OriginalFullText = fileContent,
                StringsCount = 0,
                Version = 0,
                Priority = 0,
                IsFolder = false,
                ID_FolderOwner = parentId,
                //TODO: file encoding
                Encoding = "",
                ID_LocalizationProject = projectId,
            };

            return await this.AddNode(newFile, insertToDbAction: this.InsertFileToDbAsync);
        }

        public async Task<Node<File>> AddFolder(FolderModel newFolderModel)
        {
            var foundedFolder = await this._filesRepository.GetByNameAndParentId(newFolderModel.Name, newFolderModel.ParentId);
            if (foundedFolder != null)
            {
                throw new Exception($"Folder \"{newFolderModel.Name}\" already exists");
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
            // var foundedFile = await filesRepository.GetByID(id);
            // if (foundedFile == null)
            // {
            //     throw new Exception($"File by id \"{id}\" not found");
            // }

            file.ID = id;
            file.DateOfChange = DateTime.Now;
            var updatedSuccessfully = await this._filesRepository.UpdateAsync(file);
            if (!updatedSuccessfully)
            {
                throw new Exception($"Failed to update file with id \"{id}\" in database");
            }
        }

        public async Task DeleteNode(int id)
        {
            // Check if file by id exists in database
            // var foundedFile = await filesRepository.GetByID(id);
            // if (foundedFile == null)
            // {
            //     throw new Exception($"File by id \"{id}\" not found");
            // }

            var deleteSuccessfully = await this._filesRepository.RemoveAsync(id);
            if (!deleteSuccessfully)
            {
                throw new Exception($"Failed to remove file with id \"{id}\" from database");
            }
        }

        private async Task<Node<File>> AddNode(File file, Func<File, Task> insertToDbAction)
        {
            if (file.ID_FolderOwner.HasValue)
            {
                var parentFile = await this._filesRepository.GetByIDAsync(file.ID_FolderOwner.Value);
                if (parentFile?.IsFolder == false)
                {
                    throw new Exception($"Can not add new node \"{file.Name}\" with file as parent node");
                }
            }

            await insertToDbAction(file);

            var addedFile = await this._filesRepository.GetByNameAndParentId(file.Name, file.ID_FolderOwner);
            var icon = GetIconByFile(addedFile);
            return new Node<File>(addedFile, icon);
        }

        private async Task InsertFileToDbAsync(File file)
        {
            var fileUploaded = await this._filesRepository.Upload(file);
            if (!fileUploaded)
            {
                throw new Exception($"Failed to insert file \"{file.Name}\" in database");
            }
        }

        private async Task InsertFolderToDbAsync(File file)
        {
            var folderAdded = await this._filesRepository.AddAsync(file) > 0;
            if (!folderAdded)
            {
                throw new Exception($"Failed to insert folder \"{file.Name}\" in database");
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
