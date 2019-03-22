using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class TranslationTopicController : ControllerBase
    {
        private readonly ITranslationTopicRepository _translationTopicRepository;

        public TranslationTopicController(ITranslationTopicRepository translationTopicRepository)
        {
            _translationTopicRepository = translationTopicRepository;
        }


        /// <summary>
        /// Возвращает список тематик, содержащий только идентификатор и наименование.
        /// Для выборки, например checkbox.
        /// </summary>
        /// <returns>{id, name_text}</returns>
        [Authorize]
        [HttpPost("forSelect")]
        public async Task<IEnumerable<TranslationTopicForSelectDTO>> GetAllForSelectAsync()
        {
            return await _translationTopicRepository.GetAllForSelectAsync();
        }
    }
}
