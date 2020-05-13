using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class ProjectLocaleController : ControllerBase
    {

        private readonly LocalizationProjectsLocalesRepository _localizationProjectsLocalesRepository;
        private readonly UserActionRepository _userActionRepository;

        public ProjectLocaleController()
        {
            _localizationProjectsLocalesRepository = new LocalizationProjectsLocalesRepository(Settings.GetStringDB());
            _userActionRepository = new UserActionRepository(Settings.GetStringDB());
        }

        /// <summary>
        /// Возвращает назначенные языки переводов на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("listByProjectId/{projectId}")]
        public async Task<IEnumerable<LocalizationProjectsLocales>> GetAllByProjectId(Guid projectId)
        {
            return await _localizationProjectsLocalesRepository.GetAllByProjectId(projectId);
        }

        /// <summary>
        /// Переназначение языков переводов на проект локализации.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("editProjectLocales/{projectId}")]
        public async Task UpdateProjectsLocales(Guid projectId, [FromBody] LocalizationProjectsLocales[] projectLocales)
        {
            await _localizationProjectsLocalesRepository.UpdateProjectLocales(projectId, projectLocales);
        }

        /// <summary>
        /// Назначение языков переводов на проект локализации.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create")]
        public async Task CreateProjectsLocales([FromBody] LocalizationProjectsLocales[] projectLocales)
        {
            await _localizationProjectsLocalesRepository.CreateProjectLocales(projectLocales);
        }
        
    }
}
