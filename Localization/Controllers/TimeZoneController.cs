﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class TimeZoneController : ControllerBase
    {
        private readonly TimeZoneRepository _timeZoneRepository;

        public TimeZoneController()
        {
            _timeZoneRepository = new TimeZoneRepository(Settings.GetStringDB());
        }

        [Authorize]
        [HttpPost]
        public async Task<IEnumerable<TimeZone>> GetAll()
        {
            return await _timeZoneRepository.GetAllAsync();
        }
    }
}
