using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Models.DatabaseEntities;
using Models.DatabaseEntities.PartialEntities.Translations;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly TranslationRepository translationRepository;
        private readonly UserActionRepository _userActionRepository;

        public TranslationController()
        {
            translationRepository = new TranslationRepository(Settings.GetStringDB());
        }

        /// <summary>
        /// Создание варианта перевода
        /// </summary>
        /// <param name="translation">вариант перевода который необходимо добавить</param>
        /// <returns>Статус ответа</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Translation translation)
        {
            if (translation == null)
            {
                return BadRequest("Запрос с пустыми параметрами");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Модель не соответсвует");
            }

            int insertedTranslationId = await translationRepository.AddAsync(translation);
            //_userActionRepository.AddAddTraslationActionAsync(translation.ID_User, 0, translation.ID, translation.ID_String, translation.ID_Locale) ;//TODO поменять идентификатор проекта
            return Ok(insertedTranslationId);
        }

        /// <summary>
        /// Получить все варианты перевода всех фраз
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
        [HttpGet]
        [Route("InString/{idString}")]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslationsInString(int idString)
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
        /// Удалить вариант перевода
        /// </summary>
        /// <param name="idTranslation">id варианта перевода, который необходимо удалить</param>
        /// <returns>Статус ответа</returns>
        [HttpDelete]
        [Route("DeleteTranslation/{idTranslation}")]
        public async Task<IActionResult> DeleteTranslate(int idTranslation)
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

            //_userActionRepository.AddDeleteTranslationActionAsync(300, 0, idTranslation);//TODO поменять на пользователя когда будет реализована авторизация

            return Ok();
        }

        /// <summary>
        /// Подтвердить вариант перевода (поставить галочку)
        /// </summary>
        /// <param name="idTranslation">id варианта перевода</param>
        /// <returns></returns>
        [HttpPut]
        [Route("AcceptTranslation/{idTranslation}")]
        public async Task<IActionResult> AcceptTranslate(int idTranslation)
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

            //_userActionRepository.AddConfirmTranslationActionAsync(300, 0, idTranslation);//TODO поменять на пользователя когда будет реализована авторизация
            return Ok();
        }

        /// <summary>
        /// Отклонить вариант перевода (убрать галочку)
        /// </summary>
        /// <param name="idTranslation">id варианта перевода, который нужно отклонить</param>
        /// <returns></returns>
        [HttpPut]
        [Route("RejectTranslation/{idTranslation}")]
        public async Task<IActionResult> RejectTranslate(int idTranslation)
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
            //_userActionRepository.AddUpdateTranslationActionAsync(300, 0, foundedTranslation.ID, foundedTranslation.ID_String, foundedTranslation.ID_Locale, "Убрали галочку");
            return Ok();
        }

        /// <summary>
        /// Обновить вариант перевода
        /// </summary>
        /// <param name="translationId">id варианта перевода, который нужно обновить</param>
        /// <param name="updatedTranslation">вариант перевода с обновленными данными</param>
        /// <returns></returns>
        [HttpPut("{translationId}")]
        public async Task<IActionResult> UpdateTranslation(int translationId, [FromBody] Translation updatedTranslation)
        {
            updatedTranslation.ID = translationId;
            var updatedSuccessfuly = await translationRepository.UpdateAsync(updatedTranslation);
            if (!updatedSuccessfuly)
                return this.BadRequest();
            //_userActionRepository.AddUpdateTranslationActionAsync(300, 0, translationId, updatedTranslation.ID_String, updatedTranslation.ID_Locale);//TODO поменять на пользователя когда будет реализована авторизация
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
        public async Task<ActionResult<IEnumerable<TranslationWithFile>>> FindTranslationByMemory(int currentProjectId, string translationText)
        {
            if (translationText == null || translationText == "")
            {
                return NotFound($"Запрашиваемый вариант перевода пуст");
            }

            var translations = await translationRepository.GetAllTranslationsByMemory(currentProjectId, translationText);
            return Ok(translations);
        }

        //[HttpPost]
        //[Route("littleTest/{currentProjectId}")]
        //public async Task LittleTest(int currentProjectId)
        //{
        //    Ok();
        //}

        /// <summary>
        /// Поиск схожих вариантов перевода в данном проекте
        /// </summary>
        /// <param name="currentProjectId">id проекта в котором происходит поиск</param>
        /// <param name="translationSubstring">фраза для которой происходит поиск совпадений</param>
        /// <returns></returns>
        [HttpPost]
        [Route("FindSimilarTranslations/{currentProjectId}")]
        public async Task<ActionResult<IEnumerable<SimilarTranslation>>> FindSimilarTranslations(int currentProjectId, [FromBody] TranslationSubstring translationSubstring)
        {
            var similarTranslations = await translationRepository.GetSimilarTranslationsAsync(currentProjectId, translationSubstring);
            return Ok(similarTranslations);
        }

    }
}
