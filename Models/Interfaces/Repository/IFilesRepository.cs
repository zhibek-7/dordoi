using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IFilesRepository : IRepositoryAuthorizeAsync<File>
    {
        Task<bool> UploadAsync(File item, IEnumerable<Locale> locales);
        Task<IEnumerable<File>> GetByProjectIdAsync(Guid projectId, string fileNamesSearch);
        Task<File> GetLastVersionByNameAndParentIdAsync(string name, Guid? parentIdб, Guid projectId);
        IEnumerable<File> GetInitialFolders(Guid projectId);
        Task ChangeParentFolderAsync(Guid fileId, Guid? newParentId);
        Task AddTranslationLocalesAsync(Guid fileId, IEnumerable<Guid> localesIds);
        Task<IEnumerable<Locale>> GetLocalesForFileAsync(Guid fileId);
        Task DeleteTranslationLocalesAsync(Guid fileId);
        Task<string> GetFileContentAsync(Guid id, Guid? id_locale = null);
        Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoByIdAsync(Guid fileId);
        Task<IEnumerable<File>> GetFilesByParentFolderIdAsync(Guid parentFolderId);
        /// <summary>
        /// Для внутреннего использования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<File> GetByIDAsync(Guid? id);

    }


}
