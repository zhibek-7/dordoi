using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using DAL.Reposity.PostgreSqlRepository;
using Localization.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Newtonsoft.Json;

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

        public async Task<ClaimsIdentity> GetUserWithIdentity(string username, string password)
        {
            User user = new User
            {
                Name_text = username,
                Email = username,
                Password_text = password,
            };

            user = await userRepository.LoginAsync(user);
            user.Role = "Переводчик";

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name_text),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;        
        }

        //[HttpPost("Profile:{id}")]
        //public async Task<UserProfileForEditingDTO> GetProfile(int id)
        //{
        //    return await userRepository.GetProfileAsync(id);
        //}

        [Authorize]
        [HttpGet("Profile")]
        public async Task<UserProfileForEditingDTO> GetProfile()
        {
            // лезь в бд теперь по username пользователя, а не по id 
            var username = User.Identity.Name;
            return await userRepository.GetProfileAsync(301); // сюда посылай username 
        }

        [HttpPost("toSaveEdited")]
        public async Task EditGlossaryAsync(UserProfileForEditingDTO user)
        {
            await userRepository.UpdateAsync(user);
        }

        [HttpDelete("delete/{id}")]
        public async Task RemoveAsync(int id)
        {
            await userRepository.RemoveAsync(id);
        }   

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]User user)
        {
            var username = user.Name_text;
            var password = user.Password_text;           

            var identity = await this.GetUserWithIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                //await Response.WriteAsync("Invalid username or password.");
                return BadRequest();
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthenticationOptions.ISSUER,
                    audience: AuthenticationOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthenticationOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);

            // сериализация ответа
            //Response.ContentType = "application/json";
            //await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
       
    }
}
