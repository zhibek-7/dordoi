using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserRepository userRepository;

        public RoleController()
        {
            userRepository = new UserRepository(Settings.GetStringDB());
        }

        [Authorize]
        [HttpPost("list")]
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            var identityName = User.Identity.Name;
            int? userId = (int)userRepository.GetID(identityName);

            return await this._roleRepository.GetAllAsync(userId, null);
        }

    }
}
