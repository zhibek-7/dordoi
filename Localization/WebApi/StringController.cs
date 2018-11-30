using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Context; // EF
using DAL.Reposity.PostgreSqlRepository; // Native
using Models.DatabaseEntities;
using Microsoft.AspNetCore.Cors;

namespace Localization.WebApi
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StringController : ControllerBase
    {
        private readonly StringRepository stringRepository;

        public StringController()
        {
            stringRepository = new StringRepository();
        }

        [HttpGet]
        public List<Models.DatabaseEntities.String> GetStrings()
        {
            List<Models.DatabaseEntities.String> strings = stringRepository.GetAll().ToList();
            return strings;
        }

        [HttpGet("{id}")]
        //[EnableCors("AllowSpecificOrigin")]
        public Models.DatabaseEntities.String GetStringById(int id)
        {
            Models.DatabaseEntities.String foundedString = stringRepository.GetByID(id);
            return foundedString;
        }

    }
}