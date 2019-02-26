using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private RoleRepository _roleRepository = new RoleRepository(Settings.GetStringDB());

        [HttpPost("list")]
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await this._roleRepository.GetAllAsync();
        }

    }
}
