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

namespace Localization.WebApi
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StringController : ControllerBase
    {
        private readonly ITranslationSubstringRepository stringRepository;

        public StringController(ITranslationSubstringRepository translationSubstringRepository)
        {
            this.stringRepository = translationSubstringRepository;
        }

        /// <summary>
        /// GET api/strings       
        /// </summary>
        /// <returns>Список всех фраз</returns>
        [HttpGet]
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
        [HttpGet("InFile/{idFile}")]
        public async Task<ActionResult<IEnumerable<TranslationSubstring>>> GetStringsInFile(int idFile)
        {
            var strings = await stringRepository.GetStringsByFileIdAsync(idFile);

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
        [HttpGet("{id}")]
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
        [HttpPost("GetImagesByStringId/{translationSubstringId}")]
        public async Task<ActionResult<IEnumerable<Image>>> GetImagesOfTranslationSubstring(int translationSubstringId)
        {
            // Check if comment by id exists in database
            var foundedString = await stringRepository.GetByIDAsync(translationSubstringId);

            if (foundedString == null)
            {
                return NotFound($"TranslationSubstring by id \"{ translationSubstringId }\" not found");
            }

            IEnumerable<Image> images = await stringRepository.GetImagesOfTranslationSubstringAsync(translationSubstringId);

            return Ok(images);
        }

        /// <summary>
        /// Загружает картинку прикрепленную к комментарию
        /// </summary>
        /// <param name="TranslationSubstringId">id строки для перевода к которому приложена картинка</param>
        /// <returns></returns>
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
                        img.ID_User = 301;
                        img.Name = fileName;
                        img.DateTimeAdded = DateTime.Now;
                        img.Data = imageData;

                        int insertedCommentId = await stringRepository.UploadImageAsync(img, Convert.ToInt32(translationSubstringId));
                    }
                }


            }
            return Ok();
        }

        [HttpGet("ByProjectId/{projectId}")]
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

        [HttpDelete("{translationSubstringId}")]
        public async Task DeleteTranslationSubstring(int translationSubstringId)
        {
            await this.stringRepository.RemoveAsync(id: translationSubstringId);
        }

        [HttpPut("{translationSubstringId}")]
        public async Task UpdateTranslationSubstring(int translationSubstringId, [FromBody] TranslationSubstring updatedTranslationSubstring)
        {
            updatedTranslationSubstring.ID = translationSubstringId;
            await this.stringRepository.UpdateAsync(item: updatedTranslationSubstring);
        }

        [HttpGet("{translationSubstringId}/locales")]
        public async Task<IEnumerable<Locale>> GetLocalesIdsForStringAsync(int translationSubstringId)
        {
            return await this.stringRepository.GetLocalesForStringAsync(translationSubstringId: translationSubstringId);
        }

        [HttpPut("{translationSubstringId}/locales")]
        public async Task UpdateLocalesForStringAsync(int translationSubstringId, [FromBody] IEnumerable<int> localesIds)
        {
            await this.stringRepository.DeleteTranslationLocalesAsync(translationSubstringId: translationSubstringId);
            await this.stringRepository.AddTranslationLocalesAsync(translationSubstringId: translationSubstringId, localesIds: localesIds);
        }

    }
}
