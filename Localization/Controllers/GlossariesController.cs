using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities.DTO;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossariesController : ControllerBase
    {
        private readonly GlossariesService _glossariesService;
        private readonly UserActionRepository _userActionRepository;

        public GlossariesController(GlossariesService glossariesService)
        {
            _glossariesService = glossariesService;
            _userActionRepository = new UserActionRepository(Settings.GetStringDB());
        }


        /// <summary>
        /// Возвращает список глоссариев, со строками перечислений имен связанных объектов.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllToDTOAsync()
        {
            return await _glossariesService.GetAllToDTOAsync();
        }

        /// <summary>
        /// Добавление нового глоссария.
        /// </summary>
        /// <param name="glossary">Новый глоссарий.</param>
        /// <returns></returns>
        [HttpPost("newGlossary")]
        public async Task AddGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            _userActionRepository.AddCreateGlossaryActionAsync(300, "Test user", glossary.id, glossary.id);//TODO поменять на пользователя когда будет реализована авторизация
            await _glossariesService.AddNewGlossaryAsync(glossary);
        }

        /// <summary>
        /// Возвращает глоссарий для редактирования (со связанными объектами).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<GlossariesForEditingDTO> GetGlossaryForEditAsync([FromBody] int glossaryId)
        {
            return await _glossariesService.GetGlossaryForEditAsync(glossaryId);
        }

        /// <summary>
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий.</param>
        /// <returns></returns>
        [HttpPost("editSaveGlossary")]
        public async Task EditGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            _userActionRepository.AddEditGlossaryActionAsync(300, "Test user", glossary.id, glossary.id);//TODO поменять на пользователя когда будет реализована авторизация
            await _glossariesService.EditGlossaryAsync(glossary);
        }

        /// <summary>
        /// Удаление глоссария.
        /// </summary>
        /// <param name="id">Идентификатор глоссария.</param>
        /// <returns></returns>
        [HttpDelete("deleteGlossary/{glossaryId}")]
        public async Task DeleteGlossaryAsync(int glossaryId)
        {
            _userActionRepository.AddDeleteGlossaryActionAsync(300, "Test user", glossaryId, glossaryId);//TODO поменять на пользователя когда будет реализована авторизация
            await _glossariesService.DeleteGlossaryAsync(glossaryId);
        }

        /// <summary>
        /// Удаление всех терминов глоссария.
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        [HttpDelete("clearGlossary/{glossaryId}")]
        public async Task ClearGlossaryAsync(int glossaryId)
        {
            await _glossariesService.ClearGlossaryAsync(glossaryId);
        }

    }
}
