using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly LocalizationProjectRepository _localizationProjectRepository;
        private readonly LocalizationProjectsLocalesRepository _localizationProjectsLocalesRepository;
        private readonly LocaleRepository _localeRepository;
        private readonly UserActionRepository _userActionRepository;
        private readonly UserRepository _userRepository;

        public ProjectController()
        {
            _localizationProjectRepository = new LocalizationProjectRepository(Settings.GetStringDB());
            _localizationProjectsLocalesRepository = new LocalizationProjectsLocalesRepository(Settings.GetStringDB());
            _localeRepository = new LocaleRepository(Settings.GetStringDB());
            _userActionRepository = new UserActionRepository(Settings.GetStringDB());
            _userRepository = new UserRepository(Settings.GetStringDB());
        }

        //[Authorize]
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


  

        [HttpPost]
        [Route("newProject")]
        public async Task<int> newProject([FromBody] LocalizationProject project)
        {
          int idProj= await _localizationProjectRepository.AddAsyncInsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync(300, "Test user", project.id, project.ID_Source_Locale);//TODO поменять на пользователя когда будет реализована авторизация
            return idProj;
        }

  
        [HttpPost]
        [Route("AddProject")]
        public LocalizationProject AddProject([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync(300, "Test user", project.id, project.ID_Source_Locale);//TODO поменять на пользователя когда будет реализована авторизация
            return project;
        }

        [HttpPost]
        [Route("AddProject2")]
        public LocalizationProject AddProjectT([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync(300, "Test user", project.id, project.ID_Source_Locale);//TODO поменять на пользователя когда будет реализована авторизация
            return project;
        }


        [HttpPost]
        [Route("deleteLocales/{Id}")]
        public void deleteLocalesById(int Id)
        {
            _localizationProjectsLocalesRepository.DeleteProjectLocalesById(Id);
        }
        [HttpPost]
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
            _userActionRepository.AddEditProjectActionAsync(300, "Test user", project.id, project.ID_Source_Locale);//TODO поменять на пользователя когда будет реализована авторизация           

            return project;
        }

        /// <summary>
        /// Возвращает список проектов локализации, назначенных на пользователя
        /// </summary>
        /// <returns>LocalizationProjectForSelectDTO{ID, Name}</returns>
        [Authorize]
        [HttpPost("forSelectByUser")]
        public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetForSelectByUserAsync()
        {
            var userName = User.Identity.Name;
            return await _localizationProjectRepository.GetForSelectByUserAsync(userName);
        }

        /// <summary>
        /// добавляет языки
        /// </summary>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddProjectLocale")]
        public LocalizationProjectsLocales[] EditProjectLocales([FromBody] LocalizationProjectsLocales[] projectLocales)
        {
            foreach (LocalizationProjectsLocales projectLocale in projectLocales)
            {
                try
                {
                    _localizationProjectsLocalesRepository.AddProjectsLocales(projectLocale);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("{0} Exception caught.", exception);
                }
            }
            return projectLocales;
        }


        /// <summary>
        /// взвращает все языки по id проекта
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ListProjectLocales/{Id}")]
        public Task<IEnumerable<LocalizationProjectsLocales>> GetProjectsLocales(int Id)
        {
            return _localizationProjectsLocalesRepository.GetAll(Id);
        }

        /// <summary>
        /// взвращает все языки
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ListLocales")]
        public Task<IEnumerable<Locale>> GetLocales()
        {

            return _localeRepository.GetAllAsync();
        }




        /// <summary>
        /// обновляет языки
        /// </summary>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteProjectLocale")]
        public LocalizationProjectsLocales[] DeleteProjectsLocales([FromBody] LocalizationProjectsLocales[] projectLocales)
        {
            foreach (LocalizationProjectsLocales projectLocale in projectLocales)
            {
                try
                {
                    _localizationProjectsLocalesRepository.DeleteProjectsLocales(projectLocale);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("{0} Exception caught.", exception);
                }
            }
            return projectLocales;
        }

        /// <summary>
        /// обновляет языки
        /// </summary>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("editProjectLocale")]
        public LocalizationProjectsLocales[] EditProjectsLocales([FromBody] LocalizationProjectsLocales[] projectLocales)
        {
            foreach (LocalizationProjectsLocales projectLocale in projectLocales)
            {
                try
                {
                    _localizationProjectsLocalesRepository.UpdateProjectsLocales(projectLocale);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("{0} Exception caught.", exception);
                }
            }
            return projectLocales;
        }




    }
}
