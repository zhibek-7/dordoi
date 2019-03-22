using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationMemoryController : ControllerBase
    {
        private readonly TranslationMemoryService _translationMemoryService;
        private UserRepository ur;

        public TranslationMemoryController(TranslationMemoryService translationMemoryService)
        {
            _translationMemoryService = translationMemoryService;
            var connectionString = Settings.GetStringDB();
            ur = new UserRepository(connectionString);
        }


        /// <summary>
        /// Возвращает список памяти переводов текущего пользователя (со связанными объектами) и их количество.
        /// </summary>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
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
                value: (await _translationMemoryService.GetAllByUserIdCountAsync(
                    userId: userId,
                    projectId: projectId,
                    searchString: searchString
                )).ToString());

            var strings = await _translationMemoryService.GetAllByUserIdAsync(
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
                return BadRequest("Translation_memory not found");
            }

            return Ok(strings);
        }


        /// <summary>
        /// Возвращает список памятей переводов назначенных на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns>TranslationMemoryForSelectDTO</returns>
        [Authorize]
        [HttpPost("forSelectByProject/{projectId}")]
        public async Task<IEnumerable<TranslationMemoryForSelectDTO>> GetForSelectByProjectAsync(Guid projectId)
        {
            return await _translationMemoryService.GetForSelectByProjectAsync(projectId);
        }


        /// <summary>
        /// Добавление новой памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Новая память переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create")]
        public async Task AddAsync(TranslationMemoryForEditingDTO translationMemory)
        {
            var userId = (Guid)ur.GetID(User.Identity.Name);
            await _translationMemoryService.AddAsync(userId, translationMemory);
        }

        /// <summary>
        /// Возвращает память переводов для редактирования (со связанными объектами).
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("edit/{id}")]
        public async Task<TranslationMemoryForEditingDTO> GetForEditAsync(Guid id)
        {
            return await _translationMemoryService.GetForEditAsync(id);
        }

        /// <summary>
        /// Сохранение изменений в памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Отредактированная память переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("editSave")]
        public async Task UpdateAsync(TranslationMemoryForEditingDTO translationMemory)
        {
            var userId = (Guid)ur.GetID(User.Identity.Name);
            await _translationMemoryService.UpdateAsync(userId, translationMemory);
        }

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _translationMemoryService.DeleteAsync(id);
        }

        /// <summary>
        /// Удаление всех записей из памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("clear/{id}")]
        public async Task<bool> ClearAsync(Guid id)
        {
            return await _translationMemoryService.ClearAsync(id);
        }
    }
}
