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
        private UserRepository ur;
        private readonly ParticipantRepository _participantsRepository;
        private RoleRepository _roleRepository;

        public ProjectController()
        {
            string connectionString = Settings.GetStringDB();
            _localizationProjectRepository = new LocalizationProjectRepository(connectionString);
            _localizationProjectsLocalesRepository = new LocalizationProjectsLocalesRepository(connectionString);
            _localeRepository = new LocaleRepository(connectionString);
            _userActionRepository = new UserActionRepository(connectionString);
            ur = new UserRepository(connectionString);
            _participantsRepository = new ParticipantRepository(connectionString);
            _roleRepository = new RoleRepository(connectionString);
        }

        [Authorize]
        [HttpPost]
        [Route("List")]
        public List<LocalizationProject> GetProjects()
        {

            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);
            return _localizationProjectRepository.GetAllAsync(userId, null).Result?.ToList();
        }

        [Authorize]
        [HttpPost]
        [Route("{Id}")]
        public LocalizationProject GetProjectById(Guid Id)
        {
            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);
            var project = _localizationProjectRepository.GetByIDAsync(Id, userId);
            return project.Result;
        }

        /// <summary>
        /// Возвращает проект локализации с подробной иформацией из связанных данных.
        /// </summary>
        /// <param name="id">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("details/{Id}")]
        public async Task<LocalizationProject> GetWithDetailsById(Guid id)
        {
            return await _localizationProjectRepository.GetWithDetailsById(id);
        }


        //[HttpPost]
        //[Route("add/{project}")]
        [Authorize]
        [HttpPost]
        [Route("newProject")]
        public async Task<Guid?> newProject([FromBody] LocalizationProject project)
        {
            Guid? idProj = await _localizationProjectRepository.AddAsync(project);
            await _userActionRepository.AddCreateProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, project.id, project.ID_Source_Locale);
            return idProj;
        }

        //[HttpPost]
        //[Route("add/{project}")]
        [Authorize]
        [HttpPost]
        [Route("AddProject")]
        public async Task<LocalizationProject> AddProject([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            await _userActionRepository.AddCreateProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, project.id, project.ID_Source_Locale);
            return project;
        }

        [Authorize]
        [HttpPost]
        [Route("AddProject2")]
        public LocalizationProject AddProjectT([FromBody] LocalizationProject project)
        {
            _localizationProjectRepository.InsertProject(project);
            _userActionRepository.AddCreateProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, project.id, project.ID_Source_Locale);
            return project;
        }

        [Authorize]
        [HttpPost]
        [Route("deleteLocales/{Id}")]
        public void deleteLocalesById(Guid Id)
        {
            _localizationProjectsLocalesRepository.DeleteProjectLocalesById(Id);
        }

        [Authorize]
        [HttpPost]
        [Route("delete/{Id}")]
        public void DeleteProject(Guid Id)
        {
            _localizationProjectRepository.RemoveAsync(Id);
        }

        [Authorize]
        [HttpPost]
        [Route("edit/{Id}")]
        public async Task<LocalizationProject> EditProject(LocalizationProject project, Guid Id)
        {
            _localizationProjectRepository.Update(project);
            await _userActionRepository.AddEditProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, project.id, project.ID_Source_Locale);

            return project;
        }



        /// <summary>
        /// Создание проекта локализации.
        /// </summary>
        /// <param name="project">Проект локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create")]
        public async Task<Guid?> Create(CreateLocalizationProject project)
        {
            var projectId = await _localizationProjectRepository.AddAsync(project);
            await _userActionRepository.AddCreateProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, (Guid)projectId, (Guid)project.ID_Source_Locale);

            var userId = (Guid)ur.GetID(User.Identity.Name);
            var roleIdOwner = (Guid)_roleRepository.GetRoleId("owner");

            var participant = new Participant
            {
                ID_Localization_Project = projectId,
                ID_User = userId,
                ID_Role = roleIdOwner,
                Active = true
            };

            await _participantsRepository.AddAsync(participant);

            return projectId;
        }

        /// <summary>
        /// Обновление данных проекта локализации.
        /// </summary>
        /// <param name="project">Проект локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("update")]
        public async Task<LocalizationProject> Update(LocalizationProject project)
        {
            await _localizationProjectRepository.UpdateAsync(project);
            await _userActionRepository.AddEditProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, project.id, project.ID_Source_Locale);

            return project;
        }

        /// <summary>
        /// Возвращает список проектов локализации, назначенных на пользователя
        /// </summary>
        /// <returns>LocalizationProjectForSelectDTO{ID, Name}</returns>
        [Authorize]
        [HttpPost]
        [Route("forSelectByUser")]
        public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetAllForSelectAsync()
        {
            var userName = User.Identity.Name;
            return await _localizationProjectRepository.GetForSelectByUserAsync(userName);
        }

        /// <summary>
        /// добавляет языки
        /// </summary>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        [Authorize]
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
        [Authorize]
        [HttpPost]
        [Route("ListProjectLocales/{Id}")]
        public Task<IEnumerable<LocalizationProjectsLocales>> GetProjectsLocales(Guid Id)
        {
            return _localizationProjectsLocalesRepository.GetAll(Id);
        }

        /// <summary>
        /// взвращает все языки
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
