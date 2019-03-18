using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities.DTO;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossariesController : ControllerBase
    {
        private readonly GlossariesService _glossariesService;
        private readonly UserActionRepository _userActionRepository;
        private UserRepository ur;

        public GlossariesController(GlossariesService glossariesService)
        {
            _glossariesService = glossariesService;
            var connectionString = Settings.GetStringDB();
            _userActionRepository = new UserActionRepository(connectionString);

            ur = new UserRepository(connectionString);

        }


        /// <summary>
        /// Возвращает список глоссариев (со связанными объектами) и их количество.
        /// </summary>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("byUserId")]
        public async Task<ActionResult<IEnumerable<TranslationSubstringTableViewDTO>>> GetAllWithTranslationMemoryByProjectAsync(
            int? offset,
            int? limit,
            int? projectId,
            string searchString,
            string[] sortBy,
            bool? sortAscending)
        {
            var identityName = User.Identity.Name;
            int? userId = (int)ur.GetID(identityName);

            Response.Headers.Add(
                key: "totalCount",
                value: (await _glossariesService.GetAllByUserIdCountAsync(
                    userId: userId,
                    projectId: projectId,
                    searchString: searchString
                )).ToString());

            var strings = await _glossariesService.GetAllByUserIdAsync(
                userId: userId,
                offset: offset ?? 0,
                limit: limit ?? 25,
                projectId: projectId,
                searchString: searchString,
                sortBy: sortBy,
                sortAscending: sortAscending ?? true
            );

            if (strings == null)
            {
                return BadRequest("Glossaries not found");
            }

            return Ok(strings);
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
        /// Добавление нового глоссария.
        /// </summary>
        /// <param name="glossary">Новый глоссарий.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("newGlossary")]
        public async Task AddGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            var identityName = User.Identity.Name;
            int? userId = (int)ur.GetID(identityName);
            await _userActionRepository.AddCreateGlossaryActionAsync((int)userId, identityName, glossary.id, glossary.Name_text);
            await _glossariesService.AddNewGlossaryAsync((int)userId,  glossary);
        }

        /// <summary>
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("editSaveGlossary")]
        public async Task EditGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            var identityName = User.Identity.Name;
            int? userId = (int)ur.GetID(identityName);
            await _userActionRepository.AddEditGlossaryActionAsync((int)userId, identityName, glossary.id, glossary.Name_text);
            await _glossariesService.EditGlossaryAsync((int)userId, glossary);
        }

        /// <summary>
        /// Удаление глоссария.
        /// </summary>
        /// <param name="id">Идентификатор глоссария.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("deleteGlossary/{glossaryId}")]
        public async Task DeleteGlossaryAsync(int glossaryId)
        {
            var identityName = User.Identity.Name;
            int? userId = (int)ur.GetID(identityName);
            await _userActionRepository.AddDeleteGlossaryActionAsync((int)userId, identityName, glossaryId, "");
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
