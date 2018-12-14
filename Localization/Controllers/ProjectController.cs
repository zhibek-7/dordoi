using System.Collections.Generic;
using System.Data;
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


        [HttpPost]
        [Route("add/{project}")]
        public LocalizationProject AddProject(LocalizationProject project)
        {
              _localizationProjectRepository.InsertProject(project);
            return project;
        }


        [HttpGet]
        [Route("delete/{Id}")]
        public void DeleteProject(int Id)
        {
         _localizationProjectRepository.DeleteProject(Id);
        }
        [HttpGet]
        [Route("edit/{Id}")]
        public LocalizationProject EditProject(LocalizationProject project, int Id)
        {
          _localizationProjectRepository.UpdateProject(project);
          return project;
        }

    }
}
