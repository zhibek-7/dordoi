using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using System.Threading.Tasks;

namespace Models.Interfaces.Repository
{
    public interface ILocaleRepository
    {
        Task<IEnumerable<Locale>> GetAllAsync();
        Task<IEnumerable<Locale>> GetAllForProject(int projectId);
        Task<IEnumerable<Locale>> GetByUserIdAsync(int userId);
    }
}
