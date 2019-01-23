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
        
        [HttpGet]
        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllToDTOAsync() //Переименовать в GetAllDTOAsync
        {
            return await _glossariesService.GetAllToDTOAsync(); //Переименовать в GetAllDTOAsync
        }

        [HttpGet("locales/list")]
        public async Task<IEnumerable<Locale>> GetLocalesAsync()
        {
            return await _localeRepository.GetAllAsync();
        }

        [HttpGet("localizationProjects/list")]
        public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetLocalizationProjectsAsync()
        {
            return await _localizationProjectRepository.GetAllForSelectDTOAsync();
        }

        [HttpPost("newGlossary")]//переименовать в addGlossary
        public async Task AddGlossaryAsync(GlossariesForEditing glossary)
        {
            await _glossariesService.AddNewGlossaryAsync(glossary);
        }

        [HttpPost("edit")]
        public async Task<GlossariesForEditing> GetGlossaryForEditAsync([FromBody] int glossaryId)
        {
            return await _glossariesService.GetGlossaryForEditAsync(glossaryId);
        }

        [HttpPost("editSaveGlossary")]
        public async Task EditGlossaryAsync(GlossariesForEditing glossary)
        {
            await _glossariesService.EditGlossaryAsync(glossary);
        }

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
