using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<Guid?> CreateUser(User user);
        IEnumerable<User> GetAll();
        IEnumerable<User> GetByProjectID(Guid Id);
        Guid? GetID(string name);
        Task<byte[]> GetPhotoByIdAsync(Guid id);
        Task<UserProfileForEditingDTO> GetProfileAsync(string name);
        Task<string> GetRoleAsync(string userName, Guid? projectId);
        Task<bool?> IsUniqueEmail(string email, string name_text = null);
        Task<bool?> IsUniqueLogin(string login);
        Task<User> LoginAsync(User user);
        Task<bool> PasswordChange(UserPasswordChangeDTO user);
        Task<bool> RecoverPassword(string name);
        Task<bool?> RemoveAsync(string name);
        Task UpdateAsync(UserProfileForEditingDTO user);
        Task UpdateUsersLocalesAsync(Guid userId, IEnumerable<Tuple<Guid, bool>> localesIdIsNative, bool isDeleteOldRecords = true);
    }
}
