using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost]
        [Route("List")]
        public async Task<IEnumerable<UserAction>> GetAll()
        {
            return await this._userActionRepository.GetAllAsync();
        }

        public class GetAllByProjectIdArgs
        {
            public int? offset { get; set; }
            public int? limit { get; set; }
            public int? workTypeId { get; set; }
            public int? userId { get; set; }
            public int? localeId { get; set; }
            public string[] sortBy { get; set; }
            public bool? sortAscending { get; set; }
        }
        /// <summary>
        /// Получить список действий пользователей на определеном проекте
        /// </summary>
        /// <param name="projectId">Идентификатор пользователя</param>
        /// <returns>Список действий</returns>
        [Authorize]
        [HttpPost]
        [Route("List/byProjectId/{projectId}")]
        public async Task<IEnumerable<UserAction>> GetAllByProjectId(int projectId, [FromBody] GetAllByProjectIdArgs param)
        {
            this.Response.Headers.Add(
                key: "totalCount",
                value: (await this._userActionRepository.GetAllByProjectIdCountAsync(
                    projectId: projectId,
                    workTypeId: param.workTypeId ?? -1,
                    userId: param.userId ?? -1,
                    localeId: param.localeId ?? -1)).ToString());
            return await this._userActionRepository.GetAllByProjectIdAsync(
                projectId: projectId,
                offset: param.offset ?? 0,
                limit: param.limit ?? 25,
                workTypeId: param.workTypeId ?? -1,
                userId: param.userId ?? -1,
                localeId: param.localeId ?? -1,
                sortBy: param.sortBy,
                sortAscending: param.sortAscending ?? true
                );
        }
    }
}
