using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int?> CreateUser(User user);
        IEnumerable<User> GetAll();
        IEnumerable<User> GetByProjectID(int Id);
        int? GetID(string name);
        Task<byte[]> GetPhotoByIdAsync(int id);
        Task<UserProfileForEditingDTO> GetProfileAsync(string name);
        Task<string> GetRoleAsync(string userName, int? projectId);
        Task<bool?> IsUniqueEmail(string email, string name_text = null);
        Task<bool?> IsUniqueLogin(string login);
        Task<User> LoginAsync(User user);
        Task<bool> PasswordChange(UserPasswordChangeDTO user);
        Task<bool> RecoverPassword(string name);
        Task<bool?> RemoveAsync(string name);
        Task UpdateAsync(UserProfileForEditingDTO user);
        Task UpdateUsersLocalesAsync(int userId, IEnumerable<Tuple<int, bool>> localesIdIsNative, bool isDeleteOldRecords = true);
    }
}
