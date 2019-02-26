using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Context; // EF
using DAL.Reposity.PostgreSqlRepository;
using Localization.Controllers;
// Native
using Models.DatabaseEntities;
using Utilities;

namespace Localization.WebApi.work_panel
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPanelController : ControllerBase
    {

        public WorkPanelController(PostgreSqlEFContext context)
        {

        }

    }
}
