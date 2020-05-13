using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using Models.DatabaseEntities.DTO;


using Models.DatabaseEntities;



namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class IndividualController : ControllerBase
    {
        private readonly  IndividualRepository _individRepository;
        private UserRepository ur;
        private RoleRepository _roleRepository;
        public IndividualController()
        {
            string connectionString = Settings.GetStringDB();
            _individRepository = new IndividualRepository(connectionString);
            ur = new UserRepository(connectionString);
            _roleRepository = new RoleRepository(connectionString);
        }
        [Authorize]
        [HttpPost]
        [Route("{Id}")]
        public Individual GetById(Guid Id)
        {
            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);
            var individ = _individRepository.GetByIDAsync(Id, userId);
            return individ.Result;
        }

        [Authorize]
        [HttpPost]
        [Route("List")]
        public List<Individual> GetProjects()
        {
            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);
            return _individRepository.GetAllAsync(userId, null).Result?.ToList();
        }


        [Authorize]
        [HttpPost("Create")]
        public async Task<Guid?> CreateAsync(IndividualDTO project)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)ur.GetID(identityName);

            project.ID_User = userId;

            var projectId = await _individRepository.CreateAsync(project);
            return projectId;
        }

   
        [Authorize]
        [HttpPost("update")]
        public async Task<Individual> UpdateAsync(Individual project)
        {
            await _individRepository.UpdateAsync(project);
            return project;
        }

        /// <summary>
        /// Удаление.
        /// </summary>
        /// <param name="individId">Идентификатор.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{individId}")]
        public async Task DeleteAsync(Guid individId)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)ur.GetID(identityName);
           
            await _individRepository.RemoveAsync(individId);
        }


    }
}
