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
        Task<File> GetByNameAndParentId(string name, int? parentId);
        IEnumerable<File> GetInitialFolders(int projectId);
    }


}
