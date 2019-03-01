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
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserRepository userRepository;

        public UserController()
        {
            userRepository = new UserRepository(Settings.GetStringDB());
        }

        [Authorize]
        [HttpPost]
        [Route("List")]
        public List<User> GetAll()
        {
            var name_text = User.Identity.Name;

            return userRepository.GetAll().ToList();
        }

        [Authorize]
        [HttpPost("List/projectId:{projectId}")]
        public List<User> GetAll(int projectId)
        {
            return userRepository.GetByProjectID(projectId).ToList();
        }

        [Authorize]
        [HttpPost("{userId}/getPhoto")]
        public async Task<byte[]> GetPhoto(int userId)
        {

            return await this.userRepository.GetPhotoByIdAsync(id: userId);
        }

        //
        //[Authorize]
        [Authorize]
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
        [HttpPost("profile")]
        public async Task<UserProfileForEditingDTO> GetProfile()
        {
            var username = User.Identity.Name;

            //var role = User.Claims.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
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
        /// Восстановление пароля.
        /// </summary>
        /// <param name="name">имя пользователя (логин) или email</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("recoverPassword:{name}")]
        public async Task<bool> RecoverPassword(string name)
        {
            return await userRepository.RecoverPassword(name);
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

        [Authorize]
        [HttpPost("setUserRoleAccordingToProject")]
        public async Task<ActionResult> SetUserRoleAccordingToProject()
        {
            var username = User.Identity.Name;
            var projectId = Request.Form["ProjectID"].ToString();

            var roleAccordingToProject = await userRepository.GetRoleAsync(username, Convert.ToInt32(projectId));

            var response = new
            {                
                role = roleAccordingToProject
            };

            return Ok(response);
        }

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="user">логин и пароль.</param>
        /// <returns></returns>
        [HttpPost("login")]
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
            AuthenticationOptions opt = new AuthenticationOptions();
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: opt.ISSUER,
                    audience: opt.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(opt.LIFETIME)),
                    signingCredentials: new SigningCredentials(opt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var claimsOfUser = identity.Claims;
            var roleClaimType = identity.RoleClaimType;
            var roles = claimsOfUser.Where(c => c.Type == ClaimTypes.Role).ToList();
            var roleValue = roles[0].Value;

            var response = new
            {
                token = encodedJwt,
                username = identity.Name,
                role = roleValue
            };

            return Ok(response);
        }

        /// <summary>
        /// Обновление токена у которого истекло время действия
        /// </summary>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public IActionResult RefreshToken()
        {
            var userName = Request.Form["userName"].ToString();
            var userRole = Request.Form["userRole"].ToString();


            if (userName == null)
            {
                return null;
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
                };
            ClaimsIdentity identity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            AuthenticationOptions opt = new AuthenticationOptions();
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: opt.ISSUER,
                    audience: opt.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(opt.LIFETIME)),
                    signingCredentials: new SigningCredentials(opt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                token = encodedJwt,
                username = identity.Name,
            };

            return Ok(response);
        }

        /// <summary>
        /// Получение пользователя с его claims (необходимые для авторизации)
        /// </summary>
        /// <param name="username"> Логин </param>
        /// <param name="password"> Пароль </param>
        /// <returns></returns>
        private async Task<ClaimsIdentity> GetUserWithIdentity(string username, string password)
        {
            User user = new User
            {
                Name_text = username,
                Email = username,
                Password_text = password,
            };

            user = await userRepository.LoginAsync(user);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name_text),
                    // Это сделано намерено, т.к. у пользователя может быть несколько проектов и в каждом проекте у него своя роль,
                    // а при первом логине пользователь еще не выбирает проект, соответственно его текущая роль будет "Наблюдатель".
                    // Как только пользователь выбирает проект локализации, роль меняется в соответсвии с текущим проектом.
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Наблюдатель")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        /// <summary>
        /// Проверка авторизации пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("checkUserAuthorisation")]
        public IActionResult CheckUserAuthorisation()
        {
            var username = User.Identity.Name;
            if (username != null)
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
