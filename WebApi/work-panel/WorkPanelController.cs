using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Context; // EF
using DAL.Reposity.PostgreSqlRepository; // Native
using Models.DatabaseEntities;

namespace Localization.WebApi.work_panel
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPanelController : ControllerBase
    {
        PostgreSqlEFContext context;
        private readonly UserRepository userRepository = new UserRepository();

        public WorkPanelController(PostgreSqlEFContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            List<User> users = context.Users.ToList(); //получаем через EF
            users = userRepository.GetAll().ToList(); // получаем через нативный sql
            return users;
        }

    }
}