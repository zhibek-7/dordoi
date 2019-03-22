using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Localization.Controllers;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly LocaleRepository _localeRepository;

        public LanguageController()
        {
            _localeRepository = new LocaleRepository(Settings.GetStringDB());
        }

        [Authorize]
        [HttpPost]
        [Route("List")]
        public async Task<IEnumerable<Locale>> GetLocales()
        {
            return await _localeRepository.GetAllAsync();
        }

        [Authorize]
        [HttpPost]
        [Route("byProjectId/{projectId}")]
        public async Task<IEnumerable<Locale>> GetProjectLocales(Guid projectId)
        {
            return await _localeRepository.GetAllForProject(projectId);
        }

        [Authorize]
        [HttpPost]
        [Route("byUserId/{userId}")]
        public async Task<IEnumerable<Locale>> GetUserLocales(Guid userId)
        {
            return await _localeRepository.GetByUserIdAsync(userId);
        }

        /// <summary>
        /// Возвращает назначенные языки перевода на проект локализации с процентами переводов по ним.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("localesWithPercentByProjectId/{projectId}")]
        public async Task<IEnumerable<LocalizationProjectsLocalesDTO>> GetAllForProjectWithPercent(Guid projectId)
        {
            return await _localeRepository.GetAllForProjectWithPercent(projectId);
        }

        /// <summary>
        /// Возвращает список языков назначенных на проекты, которые назначены на пользователя.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("localesByUserProjects")]
        public async Task<IEnumerable<Locale>> GetByUserProjectsAsync()
        {
            var userName = User.Identity.Name;
            return await _localeRepository.GetByUserProjectsAsync(userName);
        }

        /// <summary>
        /// Возвращает список языков назначенных на память переводов.
        /// </summary>
        /// <param name="idTranslationMemory">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("localesByTranslationMemory/{idTranslationMemory}")]
        public async Task<IEnumerable<Locale>> GetByTranslationMemory(Guid idTranslationMemory)
        {
            return await _localeRepository.GetByTranslationMemory(idTranslationMemory);
        }

        /// <summary>
        /// Возвращает список языков назначенных на память переводов.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <param name="idsTranslationSubstring">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("localesByIdsTranslationSubstring")]
        public async Task<IEnumerable<Locale>> GetByIdsTranslationSubstring(Guid projectId, Guid[] idsTranslationSubstring)
        {
            return await _localeRepository.GetByIdsTranslationSubstring(projectId, idsTranslationSubstring);
        }

        /// <summary>
        /// Возвращает основной язык проекта.
        /// </summary>
        /// <param name="id">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("getSourceLocaleLocalizationProject/{projectId}")]
        public async Task<Locale> GetSourceLocaleLocalizationProject(Guid projectId)
        {
            return await _localeRepository.GetSourceLocaleLocalizationProject(projectId);
        }
    }
}
