using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidsController : BaseController
    {

        [Authorize]
        [HttpPost("getNew")]
        public string GetNew()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
