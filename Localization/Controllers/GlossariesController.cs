using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossariesController : ControllerBase
    {
        private readonly GlossariesService _glossariesService;
        private readonly ILocaleRepository _localeRepository;
        private readonly ILocalizationProjectRepository _localizationProjectRepository;


        public GlossariesController(GlossariesService glossariesService)
        {
            _glossariesService = glossariesService;

            _localeRepository = new LocaleRepository(Settings.GetStringDB());
            _localizationProjectRepository = new LocalizationProjectRepository(Settings.GetStringDB());
        }


        /// <summary>
        /// Возвращает список глоссариев, со строками перечислений имен связанных объектов
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllToDTOAsync()
        {
            return await _glossariesService.GetAllToDTOAsync();
        }

        ///// <summary>
        ///// Возвращает список языков переводов
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("locales/list")]
        //public async Task<IEnumerable<Locale>> GetLocalesAsync()
        //{
        //    return await _localeRepository.GetAllAsync();
        //}

        ///// <summary>
        ///// Возвращает список проектов локализации
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("localizationProjects/list")]
        //public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetLocalizationProjectsAsync()
        //{
        //    return await _localizationProjectRepository.GetAllForSelectDTOAsync();
        //}

        /// <summary>
        /// Добавление нового глоссария
        /// </summary>
        /// <param name="glossary">Новый глоссарий</param>
        /// <returns></returns>
        [HttpPost("newGlossary")]
        public async Task AddGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            await _glossariesService.AddNewGlossaryAsync(glossary);
        }

        /// <summary>
        /// Возвращает глоссарий для редактирования (со связанными объектами)
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<GlossariesForEditingDTO> GetGlossaryForEditAsync([FromBody] int glossaryId)
        {
            return await _glossariesService.GetGlossaryForEditAsync(glossaryId);
        }

        /// <summary>
        /// Сохранение изменений в глоссарии
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий</param>
        /// <returns></returns>
        [HttpPost("editSaveGlossary")]
        public async Task EditGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            await _glossariesService.EditGlossaryAsync(glossary);
        }

        /// <summary>
        /// Удаление глоссария
        /// </summary>
        /// <param name="id">Идентификатор глоссария</param>
        /// <returns></returns>
        [HttpDelete("deleteGlossary/{glossaryId}")]
        public async Task DeleteGlossaryAsync(int glossaryId)
        {
            await _glossariesService.DeleteGlossaryAsync(glossaryId);
        }

        /// <summary>
        /// Удаление всех терминов глоссария
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        [HttpDelete("clearGlossary/{glossaryId}")]
        public async Task ClearGlossaryAsync(int glossaryId)
        {
            await _glossariesService.ClearGlossaryAsync(glossaryId);
        }

    }
}
