using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO.Participants;

namespace Models.Interfaces.Repository
{
    public interface IParticipantRepository : IRepositoryAsync<Participant>
    {
        Task AddOrActivateParticipant(Guid projectId, Guid userId, Guid roleId);
        Task<int?> GetAllByProjectIdCountAsync(Guid projectId, string search, Guid[] roleIds, Guid[] localeIds);
        Task<IEnumerable<ParticipantDTO>> GetByProjectIdAsync(Guid projectId, string search, Guid[] roleIds, Guid[] localeIds, int limit, int offset, string[] sortBy = null, bool sortAscending = true, string[] roleShort = null);
        Task<bool?> IsOwnerInAnyProject(string userName);
        Task SetInactiveAsync(Guid projectId, Guid userId);
    }
}
