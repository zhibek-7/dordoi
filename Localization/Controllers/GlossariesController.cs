using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities.DTO;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Utilities;
using System;

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
            Guid? projectId,
            string searchString,
            string[] sortBy,
            bool? sortAscending)
        {
            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);

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
        [HttpPost("edit/{glossaryId}")]
        public async Task<GlossariesForEditingDTO> GetGlossaryForEditAsync(Guid glossaryId)
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
        public async Task AddAsync(GlossariesForEditingDTO glossary)
        {
            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);
            await _userActionRepository.AddCreateGlossaryActionAsync((Guid)userId, identityName, glossary.id, glossary.Name_text);
            await _glossariesService.AddAsync((Guid)userId, glossary);
        }

        /// <summary>
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("editSaveGlossary")]
        public async Task UpdateAsync(GlossariesForEditingDTO glossary)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)ur.GetID(identityName);
            await _userActionRepository.AddEditGlossaryActionAsync(userId, identityName, glossary.id, glossary.Name_text);
            await _glossariesService.UpdateAsync(userId, glossary);
        }

        /// <summary>
        /// Удаление глоссария.
        /// </summary>
        /// <param name="id">Идентификатор глоссария.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("deleteGlossary/{glossaryId}")]
        public async Task DeleteAsync(Guid glossaryId)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)ur.GetID(identityName);
            await _userActionRepository.AddDeleteGlossaryActionAsync(userId, identityName, glossaryId, "");
            await _glossariesService.DeleteAsync(glossaryId);
        }

        /// <summary>
        /// Удаление всех терминов глоссария.
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        [HttpDelete("clearGlossary/{glossaryId}")]
        public async Task ClearAsync(Guid glossaryId)
        {
            await _glossariesService.ClearAsync(glossaryId);
        }

    }
}
