using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Localization.Controllers;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Utilities;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class LanguageController : BaseController
    {
        private readonly LocaleRepository _localeRepository;

        public LanguageController()
        {
            _localeRepository = new LocaleRepository(Settings.GetStringDB());
        }

        [HttpGet]
        [Route("List")]
        public async Task<IEnumerable<Locale>> GetLocales()
        {
            return await _localeRepository.GetAllAsync();
        }

        [HttpGet("byProjectId/{projectId}")]
        public async Task<IEnumerable<Locale>> GetProjectLocales(int projectId)
        {
            return await _localeRepository.GetAllForProject(projectId);
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<IEnumerable<Locale>> GetUserLocales(int userId)
        {
            return await _localeRepository.GetByUserIdAsync(userId);
        }

        /// <summary>
        /// Возвращает назначенные языки перевода на проект локализации с процентами переводов по ним.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [HttpPost("localesWithPercentByProjectId")]
        public async Task<IEnumerable<LocalizationProjectsLocalesDTO>> GetAllForProjectWithPercent([FromBody] int projectId)
        {
            return await _localeRepository.GetAllForProjectWithPercent(projectId);
        }
    }
}
