using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IFilesRepository
    {
        Task<int> Add(File item);
        Task<bool> Remove(int id);
        Task<bool> Update(File item);
        Task<bool> Upload(File item);

        Task<IEnumerable<File>> GetAll();
        Task<IEnumerable<File>> GetByProjectIdAsync(int projectId);
        Task<File> GetByID(int id);
        Task<File> GetByNameAndParentId(string name, int? parentId);
        IEnumerable<File> GetInitialFolders(int projectId);
    }
}
