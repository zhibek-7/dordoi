using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO.Participants;

namespace Models.Interfaces.Repository
{
    public interface IParticipantRepository : IRepositoryAsync<Participant>
    {
        Task AddOrActivateParticipant(int projectId, int userId, int roleId);
        Task<int> GetAllByProjectIdCountAsync(int projectId, string search, int[] roleIds, int[] localeIds);
        Task<IEnumerable<ParticipantDTO>> GetByProjectIdAsync(int projectId, string search, int[] roleIds, int[] localeIds, int limit, int offset, string[] sortBy = null, bool sortAscending = true, string[] roleShort = null);
        Task<bool?> IsOwnerInAnyProject(string userName);
        Task SetInactiveAsync(int projectId, int userId);
    }
}
