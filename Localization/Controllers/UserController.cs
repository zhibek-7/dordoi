using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public UserController()
        {
            userRepository = new UserRepository(Settings.GetStringDB());
        }

        [HttpPost]
        [Route("List")]
        public List<User> GetAll()
        {
            return userRepository.GetAll().ToList();
        }

        [HttpPost("List/projectId:{projectId}")]
        public List<User> GetAll(int projectId)
        {
            return userRepository.GetByProjectID(projectId).ToList();
        }



        [HttpPost("{userId}/getPhoto")]
        public async Task<byte[]> GetPhoto(int userId)

        {

            return await this.userRepository.GetPhotoByIdAsync(id: userId);

        }

        //
        [HttpPost("isUniqueEmail:{email}")]
        public async Task<bool?> IsUniqueEmail(string email)//[FromBody]
        {
            return await userRepository.IsUniqueEmail(email);
        }
        [HttpPost("isUniqueLogin:{login}")]
        public async Task<bool?> IsUniqueLogin(string login)//[FromBody]
        {
            return await userRepository.IsUniqueLogin(login);
        }
        [HttpPost("registration")]
        public async Task<int?> CreateUser(User user)
        {
            return await userRepository.CreateUser(user);
        }
        [HttpPost("login")]
        public async Task<User> Login(User user)
        {
            return await userRepository.Login(user);
        }
    }
}
