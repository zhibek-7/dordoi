using System;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IInvitationsRepository
    {
        Task<bool> AddAsync(Invitation invitation);
        Task<Invitation> GetByIdAsync(Guid id);
    }
}
