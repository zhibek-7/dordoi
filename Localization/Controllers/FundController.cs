using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.Reposity.PostgreSqlRepository;
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

        /// <summary>
        /// Создание фонда
        /// </summary>
        /// <param name="fund">фонд.</param>
        /// <returns></returns>
        //[Authorize]
        //[HttpPost("Create")]
        //public async Task<Guid?> CreateAsync(FundDTO fund)
        //{
        //    var fundId = await _fundRepository.CreateAsync(fund);
        //    //var userId = (Guid)ur.GetID(User.Identity.Name);
        //    return fundId;
        //}



        [Authorize]
        [HttpPost("Create")]
        public async Task<Guid?> CreateAsync(FundDTO project)
        {
            var projectId = await _fundRepository.CreateAsync(project);
           


            return projectId;
        }

















        /// <summary>
        /// Обновление данных .
        /// </summary>
        /// <param name="fund">fund</param>
        /// <returns></returns>
        //[Authorize]
        //[HttpPost("update")]
        //public async Task<Fu> UpdateAsync(LocalizationProject project)
        //{
        //    await _localizationProjectRepository.UpdateAsync(project);
        //    await _userActionRepository.AddEditProjectActionAsync((Guid)ur.GetID(User.Identity.Name), User.Identity.Name, project.id, project.ID_Source_Locale);

        //    return project;
        //}



    }
}
