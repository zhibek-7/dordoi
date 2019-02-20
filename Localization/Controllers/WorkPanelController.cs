using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Context; // EF
using DAL.Reposity.PostgreSqlRepository; // Native
using Models.DatabaseEntities;
using Utilities;

namespace Localization.WebApi.work_panel
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPanelController : ControllerBase
    {
        PostgreSqlEFContext context;
        private readonly UserRepository userRepository = new UserRepository(Settings.GetStringDB());
        private readonly TranslationSubstringRepository stringRepository = new TranslationSubstringRepository(Settings.GetStringDB());

        public WorkPanelController(PostgreSqlEFContext context)
        {
            this.context = context;
        }

    }
}
