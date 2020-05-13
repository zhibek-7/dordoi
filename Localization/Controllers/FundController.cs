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
    public class FundController : ControllerBase
    {
        private readonly  FundRepository _fundRepository;
        private UserRepository ur;
        private RoleRepository _roleRepository;
        public FundController()
        {
            string connectionString = Settings.GetStringDB();
            _fundRepository = new FundRepository(connectionString);
            ur = new UserRepository(connectionString);
            _roleRepository = new RoleRepository(connectionString);
        }
        [Authorize]
        [HttpPost]
        [Route("{Id}")]
        public Fund GetFundById(Guid Id)
        {
            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);
            var fund = _fundRepository.GetByIDAsync(Id, userId);
            return fund.Result;
        }

        [Authorize]
        [HttpPost]
        [Route("List")]
        public List<Fund> GetProjects()
        {
            var identityName = User.Identity.Name;
            Guid? userId = (Guid)ur.GetID(identityName);
            return _fundRepository.GetAllAsync(userId, null).Result?.ToList();
        }


        [Authorize]
        [HttpPost("Create")]
        public async Task<Guid?> CreateAsync(FundDTO project)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)ur.GetID(identityName);

            project.id_user = userId;

            var projectId = await _fundRepository.CreateAsync(project);
            return projectId;
        }

   
        [Authorize]
        [HttpPost("update")]
        public async Task<Fund> UpdateAsync(Fund project)
        {
            await _fundRepository.UpdateAsync(project);
            return project;
        }

        /// <summary>
        /// Удаление.
        /// </summary>
        /// <param name="fundId">Идентификатор.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{fundId}")]
        public async Task DeleteAsync(Guid fundId)
        {
            var identityName = User.Identity.Name;
            Guid userId = (Guid)ur.GetID(identityName);
           
            await _fundRepository.RemoveAsync(fundId);
        }


    }
}
