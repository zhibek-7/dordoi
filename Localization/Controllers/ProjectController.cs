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
        private readonly LocalizationProjectRepository _localizationProjectRepository;

        public ProjectController()
        {
            _localizationProjectRepository = new LocalizationProjectRepository();
        }

        [HttpGet]
        [Route("List")]
        public List<LocalizationProject> GetProjects()
        {
            var projects = _localizationProjectRepository.GetAll().ToList();
            return projects;
        }

        [HttpGet]
        [Route("{Id}")]
        public LocalizationProject GetProjectById(int Id)
        {
            var project = _localizationProjectRepository.GetByID(Id);
            return project;
        }
    }
}
