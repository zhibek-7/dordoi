using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
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
        /// Возвращает список памяти переводов, со строками перечислений имен связанных объектов.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IEnumerable<TranslationMemoryTableViewDTO>> GetAllDTOAsync()
        {

            var identityName = User.Identity.Name;
            int? userId = (int)ur.GetID(identityName);
            return await _translationMemoryService.GetAllDTOAsync(userId, null);
        }

        /// <summary>
        /// Возвращает список памятей переводов назначенных на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns>TranslationMemoryForSelectDTO</returns>
        [Authorize]
        [HttpPost("forSelectByProject")]
        public async Task<IEnumerable<TranslationMemoryForSelectDTO>> GetForSelectByProjectAsync([FromBody] int projectId)
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
            await _translationMemoryService.AddAsync(translationMemory);
        }

        /// <summary>
        /// Возвращает память переводов для редактирования (со связанными объектами).
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("edit")]
        public async Task<TranslationMemoryForEditingDTO> GetForEditAsync([FromBody] int id)
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
            await _translationMemoryService.UpdateAsync(translationMemory);
        }

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteAsync(int id)
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
        public async Task<bool> ClearAsync(int id)
        {
            return await _translationMemoryService.ClearAsync(id);
        }
    }
}
