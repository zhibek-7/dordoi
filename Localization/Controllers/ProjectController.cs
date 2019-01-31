﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly LocalizationProjectRepository _localizationProjectRepository;
        private readonly LocalizationProjectsLocalesRepository _localizationProjectsLocalesRepository;
        private readonly UserActionRepository _userActionRepository;

        public ProjectController()
        {
            _localizationProjectRepository = new LocalizationProjectRepository(Settings.GetStringDB());
            _userActionRepository = new UserActionRepository(Settings.GetStringDB());
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

        /// <summary>
        /// Возвращает проект локализации с подробной иформацией из связанных данных.
        /// </summary>
        /// <param name="id">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [HttpPost("details")]
        public async Task<LocalizationProject> GetWithDetailsById([FromBody] int id)
        {
            return await _localizationProjectRepository.GetWithDetailsById(id);
        }

        //[HttpPost]
        //[Route("add/{project}")]
        [HttpPost]
        [Route("AddProject")]
        public LocalizationProject AddProject([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync(300, "Test user", project.ID, project.ID_Source_Locale);//TODO поменять на пользователя когда будет реализована авторизация
            return project;
        }

        [HttpPost]
        [Route("AddProject2")]
        public LocalizationProject AddProjectT([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync(300, "Test user", project.ID, project.ID_Source_Locale);//TODO поменять на пользователя когда будет реализована авторизация
            return project;
        }


        [HttpGet]
        [Route("delete/{Id}")]
        public void DeleteProject(int Id)
        {
            _localizationProjectRepository.DeleteProject(Id);
        }
        [HttpPost]
        [Route("edit/{Id}")]
        public LocalizationProject EditProject(LocalizationProject project, int Id)
        {
            _localizationProjectRepository.UpdateProject(project);
            return project;
        }

        /// <summary>
        /// Возвращает список проектов локализации 
        /// </summary>
        /// <returns>LocalizationProjectForSelectDTO{ID, Name}</returns>
        [HttpPost("forSelect")]
        public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetAllForSelectAsync()
        {
            return await _localizationProjectRepository.GetAllForSelectDTOAsync();
        }

        [HttpPost]
        [Route("AddProjectLocale")]
        public LocalizationProjectsLocales[] EditProject(LocalizationProjectsLocales[] projectLocales, int Id)
        {
            foreach (LocalizationProjectsLocales projectLocale in projectLocales)
            {
                _localizationProjectsLocalesRepository.AddProjectsLocales(projectLocale);
            }
            return projectLocales;
        }
    }
}
