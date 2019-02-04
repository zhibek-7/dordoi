using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IFilesRepository : IRepositoryAsync<File>
    {
        Task<bool> Upload(File item);
        Task<IEnumerable<File>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<File>> GetByProjectIdAsync(int projectId, string fileNamesSearch);
        Task<File> GetLastVersionByNameAndParentId(string name, int? parentId);
        IEnumerable<File> GetInitialFolders(int projectId);
        Task ChangeParentFolderAsync(int fileId, int? newParentId);
        Task AddTranslationLocalesAsync(int fileId, IEnumerable<int> localesIds);
        Task<IEnumerable<Locale>> GetLocalesForFileAsync(int fileId);
        Task DeleteTranslationLocalesAsync(int fileId);
        Task<System.IO.FileStream> Download(int id, int id_locale = -1);
        Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoByIdAsync(int fileId);
        Task<IEnumerable<File>> GetFilesByParentFolderIdAsync(int parentFolderId);
    }


}
