using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace DAL.Reposity.PostgreSqlRepository
{
    public interface IFilesRepository
    {
        Task<int> Add(File item);
        Task<bool> Remove(int id);
        Task<bool> Update(File item);
        
        Task<IEnumerable<File>> GetAll();
        Task<File> GetByID(int id);
        Task<File> GetByNameAndParentId(string name, int? parentId);

    }
}
