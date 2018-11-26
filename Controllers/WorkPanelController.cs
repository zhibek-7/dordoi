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
        private readonly StringRepository stringRepository = new StringRepository();

        public WorkPanelController(PostgreSqlEFContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            //List<Models.DatabaseEntities.String> strings = context.Strings.ToList();
            List<Models.DatabaseEntities.String> strings = stringRepository.GetAll().ToList(); // получаем через нативный sql
            Models.DatabaseEntities.String foundedString = stringRepository.GetByID(5);
            List<User> users = context.Users.ToList(); //получаем через EF
            users = userRepository.GetAll().ToList(); // получаем через нативный sql
            return users;
        }

    }
}