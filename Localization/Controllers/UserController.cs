using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
        //[Authorize]
        [HttpPost("Photo")]
        public async Task PhotoAsync()
        {
            //var name_text = User.Identity.Name;
            //var content = Request.Form.Files;
        }

        /// <summary>
        /// Проверка уникальности email.
        /// </summary>
        /// <param name="email">введенный email.</param>
        /// <returns></returns>
        [HttpPost("isUniqueEmail:{email}")] 
        public async Task<bool?> IsUniqueEmail(string email)
        {
            var name_text = User.Identity.Name;
            return await userRepository.IsUniqueEmail(email, name_text);
        }

        /// <summary>
        /// Проверка уникальности имени пользователя (логина).
        /// </summary>
        /// <param name="login">введенное имя пользователя(логин).</param>
        /// <returns></returns>
        [HttpPost("isUniqueLogin:{login}")]
        public async Task<bool?> IsUniqueLogin(string login)
        {
            return await userRepository.IsUniqueLogin(login);
        }

        /// <summary>
        /// Регистрация. Создание пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("registration")]
        public async Task<int?> CreateUser(User user)
        {
            return await userRepository.CreateUser(user);
        }

        /// <summary>
        /// Получение профиля пользователя.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Profile")]
        public async Task<UserProfileForEditingDTO> GetProfile()
        {
            var username = User.Identity.Name;
            return await userRepository.GetProfileAsync(username);
        }

        /// <summary>
        /// Смена пароля.
        /// </summary>
        /// <param name="user">текущий и новый пароли.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("passwordChange")]
        public async Task<bool> PasswordChange(UserPasswordChangeDTO user)
        {
            user.Name_text = User.Identity.Name;
            return await userRepository.PasswordChange(user);
        }

        /// <summary>
        /// Сохранение изменений в профиле пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("toSaveEdited")]
        public async Task EditGlossaryAsync(UserProfileForEditingDTO user)
        {
            user.name_text = User.Identity.Name;
            await userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete")]
        public async Task<bool?> RemoveAsync()
        {
            var name_text = User.Identity.Name;
            return await userRepository.RemoveAsync(name_text);
        }

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="user">логин и пароль.</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]User user)
        {
            var username = user.Name_text;
            var password = user.Password_text;           

            var identity = await this.GetUserWithIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
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
}

        private async Task<ClaimsIdentity> GetUserWithIdentity(string username, string password)
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

        [HttpPost("checkUserAuthorisation")]
        public IActionResult CheckUserAuthorisation()
        {
            var username = User.Identity.Name;
            if(username != null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

    }
}
