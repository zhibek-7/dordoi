using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IFilesPackagesRepository
    {

        Task<FilePackage> GetByFileIdAsync(int fileId);

        Task<bool> AddAsync(FilePackage filePackage);

    }
}
