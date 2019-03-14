using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Microsoft.AspNetCore.Cors;
using Models.Interfaces.Repository;
using System.IO;
using DAL.Reposity.PostgreSqlRepository;
using Localization.Controllers;
using Microsoft.AspNetCore.Authorization;
using Models.DatabaseEntities.DTO;
using Utilities;

namespace Localization.WebApi
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StringController : ControllerBase
    {
        private readonly ITranslationSubstringRepository stringRepository;
        private UserRepository ur;


        public StringController(ITranslationSubstringRepository translationSubstringRepository)
        {
            this.stringRepository = translationSubstringRepository;
            string connectionString = Settings.GetStringDB();
            ur = new UserRepository(connectionString);
        }

        /// <summary> 
        /// GET api/strings        
        /// </summary> 
        /// <returns>Список всех фраз</returns> 
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TranslationSubstring>>> GetStrings()
        {
            var strings = await stringRepository.GetAllAsync();

            if (strings == null)
            {
                return BadRequest("Strings not found");
            }

            return Ok(strings);
        }        

        /// <summary> 
        /// Получить все фразы для перевода из файла 
        /// </summary> 
        /// <param name="idFile">id файла в котором производится поиск фраз для перевода</param> 
        /// <returns>список фраз для перевода из файла</returns> 
        [Authorize]
        [HttpPost]
        [Route("getStringsInFileWithByLocale")]
        public async Task<ActionResult<IEnumerable<TranslationSubstring>>> GetStringsInFile()
        {
            var fileId = Request.Form["fileId"].ToString();
            var localeId = Request.Form["localeId"].ToString();

            var strings = await stringRepository.GetStringsByFileIdAsync(Convert.ToInt32(fileId), Convert.ToInt32(localeId));

            if (strings == null)
            {
                return BadRequest("Strings not found");
            }

            return Ok(strings);
        }

        /// <summary> 
        /// // GET api/files/:id 
        /// </summary> 
        /// <param name="id">id фразы</param> 
        /// <returns>Фраза с необходимым id</returns> 
        [Authorize]
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<TranslationSubstring>> GetStringById(int id)
        {
            TranslationSubstring foundedString = await stringRepository.GetByIDAsync(id);

            if (foundedString == null)
            {
                return BadRequest("String not found");
            }

            return Ok(foundedString);
        }

        /// <summary> 
        /// Получить изображения строки для перевода 
        /// </summary> 
        /// <param name="translationSubstringId">id Строки для перевода</param> 
        /// <returns>Список изображений</returns> 
        [Authorize]
        [HttpPost("GetImagesByStringId/{translationSubstringId}")]
        public async Task<ActionResult<IEnumerable<Image>>> GetImagesOfTranslationSubstring(int translationSubstringId)
        {
            var foundedString = await stringRepository.GetByIDAsync(translationSubstringId);

            if (foundedString == null)
            {
                return NotFound($"TranslationSubstring by id \"{ translationSubstringId }\" not found");
            }

            IEnumerable<Image> images = await stringRepository.GetImagesOfTranslationSubstringAsync(translationSubstringId);

            return Ok(images);
        }

        /// <summary> 
        /// Получить статус перевода строки (перевод не предложен, перевод предложен, перевод одобрен) 
        /// </summary> 
        /// <param name="translationSubstringId">id Строки для перевода</param> 
        /// <returns>Статус перевода</returns> 
        [Authorize]
        [HttpPost("Status/{translationSubstringId}")]
        public async Task<ActionResult<string>> GetStatusOfTranslationSubstring(int translationSubstringId)
        {
            var foundedString = await stringRepository.GetByIDAsync(translationSubstringId);

            if (foundedString == null)
            {
                return NotFound($"TranslationSubstring by id \"{ translationSubstringId }\" not found");
            }

            string status = await stringRepository.GetStatusOfTranslationSubstringAsync(translationSubstringId);

            return "";
        }

        /// <summary> 
        /// Загружает картинку прикрепленную к комментарию 
        /// </summary> 
        /// <param name="TranslationSubstringId">id строки для перевода к которому приложена картинка</param> 
        /// <returns></returns> 
        [Authorize]
        [HttpPost("UploadImageToTranslationSubstring")]
        public async Task<IActionResult> UploadImage()
        {
            var content = Request.Form.Files["Image"];
            var translationSubstringId = Request.Form["TranslationSubstringId"];
            string fileName = content.FileName;
            long fileLength = content.Length;
            //Stream file = content.OpenReadStream; 

            if (content != null && fileLength > 0)
            {
                using (var readStream = content.OpenReadStream())
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов 
                    using (var binaryReader = new BinaryReader(content.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)fileLength);

                        Image img = new Image();
                        img.ID_User = (int)ur.GetID(User.Identity.Name);
                        img.Name_text = fileName;
                        img.Date_Time_Added = DateTime.Now;
                        img.body = imageData;

                        int insertedCommentId = await stringRepository.UploadImageAsync(img, Convert.ToInt32(translationSubstringId));
                    }
                }


            }
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("ByProjectId/{projectId}")]
        public async Task<ActionResult<IEnumerable<TranslationSubstring>>> GetByProjectId(
            int projectId,
            int? offset,
            int? limit,
            int? fileId,
            string searchString,
            string[] sortBy,
            bool? sortAscending)
        {
            this.Response.Headers.Add(
                key: "totalCount",
                value: (await this.stringRepository.GetByProjectIdCountAsync(
                            projectId: projectId,
                            fileId: fileId,
                            searchString: searchString
                    )).ToString());

            var strings = await stringRepository.GetByProjectIdAsync(
                projectId: projectId,
                offset: offset ?? 0,
                limit: limit ?? 25,
                fileId: fileId,
                searchString: searchString,
                sortBy: sortBy,
                sortAscending: sortAscending ?? true
                );

            if (strings == null)
            {
                return BadRequest("Strings not found");
            }

            return Ok(strings);
        }

        [Authorize]
        [HttpDelete("{translationSubstringId}")]
        public async Task DeleteTranslationSubstring(int translationSubstringId)
        {
            await this.stringRepository.RemoveAsync(id: translationSubstringId);
        }

        [Authorize]
        [HttpPut("{translationSubstringId}")]
        public async Task UpdateTranslationSubstring(int translationSubstringId, [FromBody] TranslationSubstring updatedTranslationSubstring)
        {
            updatedTranslationSubstring.id = translationSubstringId;
            await this.stringRepository.UpdateAsync(item: updatedTranslationSubstring);
        }

        [Authorize]
        [Route("{translationSubstringId}/locales")]
        [HttpPost]
        public async Task<IEnumerable<Locale>> GetLocalesIdsForStringAsync(int translationSubstringId)
        {
            return await this.stringRepository.GetLocalesForStringAsync(translationSubstringId: translationSubstringId);
        }

        [Authorize]
        [HttpPut("{translationSubstringId}/locales")]
        public async Task UpdateLocalesForStringAsync(int translationSubstringId, [FromBody] IEnumerable<int> localesIds)
        {
            await this.stringRepository.DeleteTranslationLocalesAsync(translationSubstringId: translationSubstringId);
            await this.stringRepository.AddTranslationLocalesAsync(translationSubstringId: translationSubstringId, localesIds: localesIds);
        }


        /// <summary>
        /// Возвращает строки (со связанными объектами) и их количество.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="searchString">Шаблон строки (поиск по substring_to_translate).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("getAllWithTranslationMemoryByProject")]
        public async Task<ActionResult<IEnumerable<TranslationSubstringTableViewDTO>>> GetAllWithTranslationMemoryByProjectAsync(
            int projectId,
            int? offset,
            int? limit,
            int? translationMemoryId,
            string searchString,
            string[] sortBy,
            bool? sortAscending)
            {
                Response.Headers.Add(
                    key: "totalCount",
                    value: (await stringRepository.GetAllWithTranslationMemoryByProjectCountAsync(
                        projectId: projectId,
                        translationMemoryId: translationMemoryId,
                        searchString: searchString
                    )).ToString());

                var strings = await stringRepository.GetAllWithTranslationMemoryByProjectAsync(
                    projectId: projectId,
                    offset: offset ?? 0,
                    limit: limit ?? 25,
                    translationMemoryId: translationMemoryId,
                    searchString: searchString,
                    sortBy: sortBy,
                    sortAscending: sortAscending ?? true
                );

                if (strings == null)
                {
                    return BadRequest("Strings not found");
                }

                return Ok(strings);
            }

        /// <summary>
        /// Обновление поля substring_to_translate
        /// </summary>
        /// <param name="translationSubstring"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("saveSubstringToTranslate")]
        public async Task<bool> UpdateSubstringToTranslateAsync(TranslationSubstringTableViewDTO translationSubstring)
        {
            return await stringRepository.UpdateSubstringToTranslateAsync(translationSubstring);
        }

        /// <summary>
        /// Удаление строк.
        /// </summary>
        /// <param name="ids">Идентификаторы строк.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("deleteRange")]
        public async Task<bool> DeleteRangeAsync(int[] ids) //IEnumerable<int> ids)
        {
            return await stringRepository.DeleteRangeAsync(ids);
        }

    }
}
