using System.Collections.Generic;
using System.Linq;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly LocaleRepository localeRepository;

        public ProjectController()
        {
            localeRepository = new LocaleRepository();
        }

        [HttpGet]
        [Route("List")]
        public List<Locale> GetProjects()
        {
            List<Locale> lacale = localeRepository.GetAll().ToList();
            return lacale;
        }
    }
}
