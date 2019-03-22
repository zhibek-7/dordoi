using System;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IFilesPackagesRepository
    {

        Task<FilePackage> GetByFileIdAsync(Guid fileId);

        Task<bool> AddAsync(FilePackage filePackage);

    }
}
