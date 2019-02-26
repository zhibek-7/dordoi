﻿using DAL.Reposity.PostgreSqlRepository;
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
    public class GlossariesController : BaseController
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
        [Authorize]
        [HttpPost("newGlossary")]
        public async Task AddGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            var identityName = User.Identity.Name;
            int? userId = (int)ur.GetID(identityName);
            // _userActionRepository.AddCreateGlossaryActionAsync(userId, identityName, glossary.id, glossary.id);//TODO поменять на пользователя когда будет реализована авторизация
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
        [Authorize]
        [HttpPost("editSaveGlossary")]
        public async Task EditGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            var identityName = User.Identity.Name;
            int? userId = (int)ur.GetID(identityName);
            // _userActionRepository.AddEditGlossaryActionAsync(userId, identityName, glossary.id, glossary.id);//TODO поменять на пользователя когда будет реализована авторизация
            await _glossariesService.EditGlossaryAsync(glossary);
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
            //_userActionRepository.AddDeleteGlossaryActionAsync(userId, identityName, glossaryId, glossaryId);//TODO поменять на пользователя когда будет реализована авторизация
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
