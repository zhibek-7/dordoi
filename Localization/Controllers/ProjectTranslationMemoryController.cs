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
    [ApiController]
    public class ProjectTranslationMemoryController : ControllerBase
    {
        private readonly ProjectTranslationMemoryRepository _projectTranslationMemoryRepository;

        public ProjectTranslationMemoryController()
        {
            _projectTranslationMemoryRepository = new ProjectTranslationMemoryRepository(Settings.GetStringDB());
        }

        /// <summary>
        /// Возвращает список связок проект - ((не)назначенных) памяти переводов.
        /// </summary>
        /// <param name="idProject">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ByProject")]
        public async Task<IEnumerable<ProjectTranslationMemory>> GetByProject([FromBody] int idProject)
        {
            return await _projectTranslationMemoryRepository.GetByProject(idProject);
        }

        /// <summary>
        /// Обновление связки проект - памяти переводов.
        /// </summary>
        /// <param name="idProject">Идентификатор проекта локализации.</param>
        /// <param name="idTranslationMemories">Идентификаторы памятей переводов.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("saveProjectTranslationMemories")]
        public async Task<bool> UpdateProjectTranslationMemories(int idProject, int[] idTranslationMemories)
        {
            return await _projectTranslationMemoryRepository.UpdateProjectTranslationMemories(idProject, idTranslationMemories);
        }

    }
}
