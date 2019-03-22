using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Localization.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Models.DatabaseEntities;
using Models.DatabaseEntities.PartialEntities.Translations;
using Utilities;
using Models.DatabaseEntities.PartialEntities.Translation;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly TranslationRepository translationRepository;
        private readonly UserActionRepository _userActionRepository;
        private readonly UserRepository userRepository;

        public TranslationController()
        {
            var connectionString = Settings.GetStringDB();
            translationRepository = new TranslationRepository(connectionString);
            userRepository = new UserRepository(connectionString);
            _userActionRepository = new UserActionRepository(connectionString);
        }

        /// <summary>
        /// Создание варианта перевода
        /// </summary>
        /// <param name="translation">вариант перевода который необходимо добавить</param>
        /// <returns>Статус ответа</returns>
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody]Translation translation)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)userRepository.GetID(identityName);

            translation.ID_User = userId;


            if (translation == null)
            {
                return BadRequest("Запрос с пустыми параметрами");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Модель не соответсвует");
            }

            Guid? insertedTranslationId = await translationRepository.AddAsync(translation);
            _userActionRepository.AddAddTraslationActionAsync((Guid)translation.ID_User, identityName, null, translation.id, translation.ID_String, translation.ID_Locale);
            //TODO поменять идентификатор проекта
            return Ok(insertedTranslationId);
        }

        /// <summary>
        /// Получить все варианты перевода всех фраз
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslations()
        {
            IEnumerable<Translation> translations = await translationRepository.GetAllAsync();
            return Ok(translations);
        }

        /// <summary>
        /// Получить все варианты перевода конкретной фразы
        /// </summary>
        /// <param name="idString">id фразы, переводы которой необходимы</param>
        /// <returns>Список вариантов перевода</returns>
        [Authorize]
        [HttpPost]
        [Route("InString/{idString}")]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslationsInString(Guid idString)
        {
            // Check if string by id exists in database
            //var foundedTranslation = await filesRepository.GetByID(id);

            // if (foundedFile == null)
            // {
            //     return NotFound($"File by id \"{id}\" not found");
            // }

            IEnumerable<Translation> translations = await translationRepository.GetAllTranslationsInStringByID(idString);
            return Ok(translations);
        }

        /// <summary>
        /// Получить все варианты перевода конкретной фразы с учётом языка
        /// </summary>
        /// <param name="idString">id фразы, переводы которой необходимы</param>
        /// <returns>Список вариантов перевода</returns>
        [Authorize]
        [HttpPost]
        [Route("InStringWithLocale")]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslationsInString()
        {
            var stringId = Request.Form["stringId"].ToString();
            var localeId = Request.Form["localeId"].ToString();

            IEnumerable<Translation> translations = await translationRepository.GetAllTranslationsInStringByIDByLocale(Guid.Parse(stringId), Guid.Parse(localeId));
            return Ok(translations);
        }

        [Authorize]
        [HttpPost]
        [Route("FindTranslationsInOtherLanguages")]
        public async Task<ActionResult<IEnumerable<TranslationWithLocaleText>>> GetTranslationsInOtherLanguages()
        {
            var currentProjectId = Request.Form["currentProjectId"].ToString();
            var translationSubstingId = Request.Form["translationSubstingId"].ToString();
            var localeId = Request.Form["localeId"].ToString();

            IEnumerable<TranslationWithLocaleText> translationsInOtherLanguages = await translationRepository.GetTranslationsInOtherLanguages(Guid.Parse(currentProjectId),
                Guid.Parse(translationSubstingId), Guid.Parse(localeId));
            return Ok(translationsInOtherLanguages);
        }

        /// <summary>
        /// Удалить вариант перевода
        /// </summary>
        /// <param name="idTranslation">id варианта перевода, который необходимо удалить</param>
        /// <returns>Статус ответа</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteTranslation/{idTranslation}")]
        public async Task<IActionResult> DeleteTranslate(Guid idTranslation)
        {
            //Check if file by id exists in database
            var foundedTranslation = await translationRepository.GetByIDAsync(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var deleteResult = await translationRepository.RemoveAsync(idTranslation);


            if (!deleteResult)
            {
                return BadRequest($"Failed to remove translation with id \"{ idTranslation }\" from database");
            }


            //_userActionRepository.AddDeleteTranslationActionAsync((int)ur.GetID(User.Identity.Name), User.Identity.Name, null, idTranslation);

            return Ok();
        }

        /// <summary>
        /// Подтвердить вариант перевода (поставить галочку)
        /// </summary>
        /// <param name="idTranslation">id варианта перевода</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("AcceptTranslation/{idTranslation}")]
        public async Task<IActionResult> AcceptTranslate(Guid idTranslation)
        {
            //Check if file by id exists in database
            var foundedTranslation = await translationRepository.GetByIDAsync(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var updateResult = await translationRepository.AcceptTranslation(idTranslation);

            if (!updateResult)
            {
                return BadRequest($"Failed to update translation with id \"{ idTranslation }\" from database");
            }

            //_userActionRepository.AddConfirmTranslationActionAsync((int)ur.GetID(User.Identity.Name), User.Identity.Name, null, idTranslation);

            return Ok();
        }

        /// <summary>
        /// Подтвердить финальный вариант перевода (поставить вторую галочку)
        /// </summary>
        /// <param name="idTranslation">id варианта перевода</param>
        /// <returns></returns>
        [HttpPut]
        [Route("AcceptFinalTranslation/{idTranslation}")]
        [Authorize(Roles = "Менеджер")]
        public async Task<IActionResult> AcceptFinalTranslation(Guid idTranslation)
        {
            //Check if file by id exists in database
            var foundedTranslation = await translationRepository.GetByIDAsync(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var updateResult = await translationRepository.AcceptTranslation(idTranslation, true);

            if (!updateResult)
            {
                return BadRequest($"Failed to update translation with id \"{ idTranslation }\" from database");
            }

            //_userActionRepository.AddConfirmTranslationActionAsync((int)ur.GetID(User.Identity.Name), User.Identity.Name, null, idTranslation);

            return Ok();
        }

        /// <summary>
        /// Отклонить вариант перевода (убрать галочку)
        /// </summary>
        /// <param name="idTranslation">id варианта перевода, который нужно отклонить</param>
        /// <returns></returns>
        [HttpPut]
        [Route("RejectTranslation/{idTranslation}")]
        [Authorize]
        public async Task<IActionResult> RejectTranslate(Guid idTranslation)
        {
            //Check if file by id exists in database
            var foundedTranslation = await translationRepository.GetByIDAsync(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var updateResult = await translationRepository.RejectTranslation(idTranslation);

            if (!updateResult)
            {
                return BadRequest($"Failed to update translation with id \"{ idTranslation }\" from database");
            }

            //int? pk = null;
            //_userActionRepository.AddUpdateTranslationActionAsync((int)ur.GetID(User.Identity.Name), User.Identity.Name, pk, idTranslation, foundedTranslation.ID_String, foundedTranslation.ID_Locale, "Убрали галочку");
            return Ok();
        }

        /// <summary>
        /// Отклонить финальный вариант перевода (убрать вторую галочку)
        /// </summary>
        /// <param name="idTranslation">id варианта перевода, который нужно отклонить</param>
        /// <returns></returns>
        [HttpPut]
        [Route("RejectFinalTranslation/{idTranslation}")]
        [Authorize(Roles = "Менеджер")]
        public async Task<IActionResult> RejectFinalTranslation(Guid idTranslation)
        {
            //Check if file by id exists in database
            var foundedTranslation = await translationRepository.GetByIDAsync(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var updateResult = await translationRepository.RejectTranslation(idTranslation, true);

            if (!updateResult)
            {
                return BadRequest($"Failed to update translation with id \"{ idTranslation }\" from database");
            }

            //int? pk = null;
            //_userActionRepository.AddUpdateTranslationActionAsync((int)ur.GetID(User.Identity.Name), User.Identity.Name, pk, idTranslation, foundedTranslation.ID_String, foundedTranslation.ID_Locale, "Убрали галочку");
            return Ok();
        }



        public class Translation2
        {
            public Guid id { get; set; }
        }

        /// <summary>
        /// Обновить вариант перевода
        /// </summary>
        /// <param name="translationId">id варианта перевода, который нужно обновить</param>
        /// <param name="updatedTranslation">вариант перевода с обновленными данными</param>
        /// <returns></returns>
        [HttpPut("{translationId}")]
        [Authorize]
        public async Task<IActionResult> UpdateTranslation(Guid translationId, [FromBody] Translation2 updatedTranslation)
        {
            Translation updatedTranslation3 = null;
            updatedTranslation3.ID_User = (Guid)userRepository.GetID(User.Identity.Name);

            updatedTranslation3.id = translationId;
            updatedTranslation3.ID_User = this.userRepository.GetID(this.User.Identity.Name).Value;
            updatedTranslation3.User_Name = this.User.Identity.Name;

            var updatedSuccessfuly = await translationRepository.UpdateAsync(updatedTranslation3);
            if (!updatedSuccessfuly)
                return this.BadRequest();
            _userActionRepository.AddUpdateTranslationActionAsync((Guid)userRepository.GetID(User.Identity.Name), User.Identity.Name, null, translationId, updatedTranslation3.ID_String, updatedTranslation3.ID_Locale);
            return this.Ok();
        }

        /// <summary>
        /// Найти варианты перевода по памяти переводов
        /// </summary>
        /// <param name="currentProjectId">id проекта в котором ведется работа в данный момент</param>
        /// <param name="translationText">фраза по которой производится поиск вариантов перевода</param>
        /// <returns></returns>
        [HttpPost]
        [Route("FindTranslationByMemory/{currentProjectId}/{translationText}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TranslationWithFile>>> FindTranslationByMemory(Guid currentProjectId, string translationText)
        {
            if (translationText == null || translationText == "")
            {
                return NotFound($"Запрашиваемый вариант перевода пуст");
            }

            var translations = await translationRepository.GetAllTranslationsByMemory(currentProjectId, translationText);
            return Ok(translations);
        }

        /// <summary>
        /// Поиск схожих вариантов перевода в данном проекте
        /// </summary>
        /// <param name="currentProjectId">id проекта в котором происходит поиск</param>
        /// <param name="translationSubstring">фраза для которой происходит поиск совпадений</param>
        /// <returns></returns>
        [HttpPost]
        [Route("FindSimilarTranslations/{currentProjectId}/{localeId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SimilarTranslation>>> FindSimilarTranslations(Guid currentProjectId, Guid localeId, [FromBody] TranslationSubstring translationSubstring)
        {
            var similarTranslations = await translationRepository.GetSimilarTranslationsAsync(currentProjectId, localeId, translationSubstring);
            return Ok(similarTranslations);
        }


        /// <summary>
        /// Возвращает все варианты перевода конкретной фразы с языком перевода.
        /// </summary>
        /// <param name="idString">Идентификатор фразы.</param>
        /// <returns>Список вариантов перевода</returns>
        [Authorize]
        [HttpPost("InStringWithLocale/{idString}")]
        public async Task<IEnumerable<TranslationDTO>> GetAllTranslationsInStringWithLocaleById(Guid idString)
        {
            return await translationRepository.GetAllTranslationsInStringWithLocaleByID(idString);
        }

        /// <summary>
        /// Обновление поля translated.
        /// </summary>
        /// <param name="translations"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("saveTranslated")]
        public async Task<bool> UpdateTranslatedAsync(IEnumerable<TranslationDTO> translations)
        {
            return await translationRepository.UpdateTranslatedAsync(translations);
        }

    }
}
