using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly LocaleRepository localeRepository;

        public LanguageController()
        {
            localeRepository = new LocaleRepository();
        }

        [HttpGet]
        [Route("List")]
        public List<Locale> GetLocales()
        {
            List<Locale> lacale = localeRepository.GetAll().ToList();
            return lacale;
        }
    }
}
