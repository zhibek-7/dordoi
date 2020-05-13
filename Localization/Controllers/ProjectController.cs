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
        public async Task<LocalizationProject> GetWithDetailsByIdAsync(Guid id)
        {
            return await _localizationProjectRepository.GetWithDetailsById(id);
        }
        
        /// <summary>
        /// Создание проекта локализации.
        /// </summary>
        /// <param name="project">Проект локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create")]
        public async Task<Guid?> CreateAsync(CreateLocalizationProject project)
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
        public async Task<LocalizationProject> UpdateAsync(LocalizationProject project)
        {
            await _localizationProjectRepository.UpdateAsync(project);
            await _userActionRepository.AddEditProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, project.id, project.ID_Source_Locale);

            return project;
        }

        /// <summary>
        /// Удаление проекта локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{projectId}")]
        public async Task DeleteAsync(Guid projectId)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)ur.GetID(identityName);
            //await _userActionRepository.AddDeleteProject(userId, identityName, projectId, "");
            await _localizationProjectRepository.RemoveAsync(projectId);
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
        
    }
}
