﻿using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities.DTO;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationMemoryController : ControllerBase
    {
        private readonly TranslationMemoryService _translationMemoriesService;


        public TranslationMemoryController(TranslationMemoryService translationMemoriesService)
        {
            _translationMemoriesService = translationMemoriesService;
        }


        /// <summary>
        /// Возвращает список памяти переводов, со строками перечислений имен связанных объектов.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<TranslationMemoryTableViewDTO>> GetAllDTOAsync()
        {
            return await _translationMemoriesService.GetAllDTOAsync();
        }

        /// <summary>
        /// Добавление новой памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Новая память переводов.</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task AddAsync(TranslationMemoryForEditingDTO translationMemory)
        {
            await _translationMemoriesService.AddAsync(translationMemory);
        }

        /// <summary>
        /// Возвращает память переводов для редактирования (со связанными объектами).
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<TranslationMemoryForEditingDTO> GetForEditAsync([FromBody] int id)
        {
            return await _translationMemoriesService.GetForEditAsync(id);
        }

        /// <summary>
        /// Сохранение изменений в памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Отредактированная память переводов.</param>
        /// <returns></returns>
        [HttpPost("editSave")]
        public async Task UpdateAsync(TranslationMemoryForEditingDTO translationMemory)
        {
            await _translationMemoriesService.UpdateAsync(translationMemory);
        }

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteAsync(int id)
        {
            return await _translationMemoriesService.DeleteAsync(id);
        }

        /// <summary>
        /// Удаление всех записей из памяти переводов.
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        [HttpDelete("clear/{id}")]
        public async Task<bool> ClearAsync(int id)
        {
            return await _translationMemoriesService.ClearAsync(id);
        }
    }
}
