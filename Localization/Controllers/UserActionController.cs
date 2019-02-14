using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class UserActionController : ControllerBase
    {
        private readonly IUserActionRepository _userActionRepository;

        public UserActionController(IUserActionRepository userActionRepository)
        {
            this._userActionRepository = userActionRepository;
        }

        /// <summary>
        /// Получить список всех действий пользователей по всем проектам
        /// </summary>
        /// <returns>Список действий</returns>
        [HttpPost]
        [Route("List")]
        public async Task<IEnumerable<UserAction>> GetAll()
        {
            return await this._userActionRepository.GetAllAsync();
        }

        /// <summary>
        /// Получить список действий пользователей на определеном проекте
        /// </summary>
        /// <param name="projectId">Идентификатор пользователя</param>
        /// <returns>Список действий</returns>
        [HttpPost]
        [Route("onProject/{projectId}")]
        public async Task<IEnumerable<UserAction>> GetAllByProjectID(int projectId)
        {
            return await this._userActionRepository.GetAllByProjectIdAsync(projectId);
        }
    }
}
