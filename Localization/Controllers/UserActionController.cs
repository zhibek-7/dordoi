using System;
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
    public class UserActionController : ControllerBase
    {
        private readonly UserActionRepository userActionRepository;

        public UserActionController()
        {
            userActionRepository = new UserActionRepository(Settings.GetStringDB());
        }

        /// <summary>
        /// Получить список всех действий пользователей по всем проектам
        /// </summary>
        /// <returns>Список действий</returns>
        [HttpPost]
        [Route("List")]
        public async Task<IEnumerable<UserAction>> GetAll()
        {
            return await userActionRepository.GetAllAsync();
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
            return await userActionRepository.GetAllByProjectIdAsync(projectId);
        }
    }
}
