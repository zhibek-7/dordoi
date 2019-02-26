using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IUserActionRepository : IRepositoryAsync<UserAction>
    {
        Task<int> AddAddFileActionAsync(File item, int? idTranslit, WorkTypes wt);
        Task<int> AddAddFileActionAsync(int userId, string userName, int projectId, int fileId, string comment = "");
        Task<int> AddAddStringActionAsync(int userId, string userName, int projectId, int stringId, string comment = "");
        Task<int> AddAddTraslationActionAsync(int userId, string userName, int projectId, int translationId, int stringId, int localeId, string comment = "");
        Task<int> AddAuthorizeActionAsync(int userId, string userName, string comment = "");
        Task<int> AddChoseTranslationActionAsync(int userId, string userName, int projectId, int translationId, string comment = "");
        Task<int> AddConfirmTranslationActionAsync(int userId, string userName, int? projectId, int translationId, string comment = "");
        Task<int> AddCreateProjectActionAsync(int userId, string userName, int projectId, int localeId, string comment = "");
        Task<int> AddDeleteStringActionAsync(int userId, string userName, int projectId, int stringId, string comment = "");
        Task<int> AddDeleteTranslationActionAsync(int userId, string userName, int? projectId, int translationId, string comment = "");
        Task<int> AddLoginActionAsync(int userId, string userName, string comment = "");
        Task<int> AddUpdateFileActionAsync(int userId, string userName, int projectId, int fileId, string comment = "");
        Task<int> AddUpdateStringActionAsync(int userId, string userName, int projectId, int stringId, string comment = "");
        Task<int> AddUpdateTranslationActionAsync(int userId, string userName, int? projectId, int translationId, int stringId, int localeId, string comment = "");
        Task<IEnumerable<UserAction>> GetAllByProjectIdAsync(
            int projectId,
            int offset,
            int limit,
            int workTypeId,
            int userId,
            int localeId,
            string[] sortBy,
            bool sortAscending);
        Task<int> GetAllByProjectIdCountAsync(
            int projectId,
            int workTypeId,
            int userId,
            int localeId);
    }
}
