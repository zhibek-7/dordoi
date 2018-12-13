using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Models.DatabaseEntities;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly TranslationRepository translationRepository;

        public TranslationController()
        {
            translationRepository = new TranslationRepository();
        }

        /// <summary>
        /// Создание варианта перевода
        /// </summary>
        /// <param name="translation">вариант перевода который необходимо добавить</param>
        /// <returns>Статус ответа</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Translation translation)
        {
            if(translation == null)
            {
                return BadRequest("Запрос с пустыми параметрами");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest("Модель не соответсвует");
            }

            int insertedTranslationId = await translationRepository.Add(translation);
            return Ok(insertedTranslationId);
        }

        /// <summary>
        /// Получить все варианты перевода всех фраз
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslations()
        {
            IEnumerable<Translation> translations = await translationRepository.GetAll();
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
            var foundedTranslation = await translationRepository.GetByID(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var deleteResult = await translationRepository.Remove(idTranslation);

            if (!deleteResult)
            {
                return BadRequest($"Failed to remove translation with id \"{ idTranslation }\" from database");
            }

            return Ok();
        }

        [HttpPut]
        [Route("AcceptTranslation/{idTranslation}")]
        public async Task<IActionResult> AcceptTranslate(int idTranslation)
        {
            //Check if file by id exists in database
            var foundedTranslation = await translationRepository.GetByID(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var updateResult = await translationRepository.AcceptTranslation(idTranslation);

            if (!updateResult)
            {
                return BadRequest($"Failed to update translation with id \"{ idTranslation }\" from database");
            }

            return Ok();
        }

        [HttpPut]
        [Route("RejectTranslation/{idTranslation}")]
        public async Task<IActionResult> RejectTranslate(int idTranslation)
        {
            //Check if file by id exists in database
            var foundedTranslation = await translationRepository.GetByID(idTranslation);

            if (foundedTranslation == null)
            {
                return NotFound($"Translation by id \"{idTranslation}\" not found");
            }

            var updateResult = await translationRepository.RejectTranslation(idTranslation);

            if (!updateResult)
            {
                return BadRequest($"Failed to update translation with id \"{ idTranslation }\" from database");
            }

            return Ok();
        }

        [HttpPut("{translationId}")]
        public async Task<IActionResult> UpdateTranslation(int translationId, [FromBody] Translation updatedTranslation)
        {
            updatedTranslation.ID = translationId;
            var updatedSuccessfuly = await translationRepository.Update(updatedTranslation);
            if (!updatedSuccessfuly)
                return this.BadRequest();

            return this.Ok();
        }

    }
}
