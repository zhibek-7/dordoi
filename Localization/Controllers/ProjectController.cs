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
        private readonly UserActionRepository _userActionRepository;

        public ProjectController()
        {
            _localizationProjectRepository = new LocalizationProjectRepository();
            _userActionRepository = new UserActionRepository();
        }

        [HttpGet]
        [Route("List")]
        public List<LocalizationProject> GetProjects()
        {
            return _localizationProjectRepository.GetAll()?.ToList();
        }

        [HttpGet]
        [Route("{Id}")]
        public LocalizationProject GetProjectById(int Id)
        {
            var project = _localizationProjectRepository.GetByID(Id);
            return project;
        }


        //[HttpPost]
        //[Route("add/{project}")]
        [HttpPost]
        [Route("AddProject")]
        public LocalizationProject AddProject([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync(300, "Test user", project.ID, project.ID_SourceLocale);//TODO поменять на пользователя когда будет реализована авторизация
            return project;
        }

        [HttpPost]
        [Route("AddProject2")]
        public LocalizationProject AddProjectT([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync(300, "Test user", project.ID, project.ID_SourceLocale);//TODO поменять на пользователя когда будет реализована авторизация
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
