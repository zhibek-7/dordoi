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

        [HttpPost]
        public IActionResult Create([FromBody]Translation translation)
        {
            if(translation == null)
            {
                return BadRequest();
            }
            translationRepository.Add(translation);
            return Ok(translation);
        }

        [HttpGet]
        public List<Translation> GetTranslations()
        {
            List<Translation> translations = translationRepository.GetAll().ToList();
            return translations;
        }

        [HttpGet]
        [Route("InString/{idString}")]
        public IEnumerable<Translation> GetTranslationsInString(int idString)
        {
            List<Translation> translations = translationRepository.GetAllTranslationsInStringByID(idString).ToList();
            return translations;
        }

    }
}