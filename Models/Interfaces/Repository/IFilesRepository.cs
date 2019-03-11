using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IFilesRepository : IRepositoryAuthorizeAsync<File>
    {
        Task<bool> UploadAsync(File item, IEnumerable<Locale> locales);
        Task<IEnumerable<File>> GetByProjectIdAsync(int projectId, string fileNamesSearch);
        Task<File> GetLastVersionByNameAndParentIdAsync(string name, int? parentId);
        IEnumerable<File> GetInitialFolders(int projectId);
        Task ChangeParentFolderAsync(int fileId, int? newParentId);
        Task AddTranslationLocalesAsync(int fileId, IEnumerable<int> localesIds);
        Task<IEnumerable<Locale>> GetLocalesForFileAsync(int fileId);
        Task DeleteTranslationLocalesAsync(int fileId);
        Task<string> GetFileContentAsync(int id, int id_locale = -1);
        Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoByIdAsync(int fileId);
        Task<IEnumerable<File>> GetFilesByParentFolderIdAsync(int parentFolderId);
        /// <summary>
        /// Для внутреннего использования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<File> GetByIDAsync(int id);

    }


}
