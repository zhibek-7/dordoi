using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class UserActionController : ControllerBase
    {
        private readonly UserActionRepository userActionRepository;

        public UserActionController()
        {
            userActionRepository = new UserActionRepository();
        }

        [HttpGet]
        [Route("List")]
        public async Task<IEnumerable<UserAction>> GetAll()
        {
            return await userActionRepository.GetAllAsync();
        }
    }
}
