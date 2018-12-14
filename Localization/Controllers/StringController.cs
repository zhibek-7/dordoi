using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// GET api/strings       
        /// </summary>
        /// <returns>Список всех фраз</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.DatabaseEntities.String>>> GetStrings()
        {
            var strings = await stringRepository.GetAllAsync();

            if (strings == null)
            {
                return BadRequest("Strings not found");
            }

            return Ok(strings);
        }

        /// <summary>
        /// // GET api/files/:id
        /// </summary>
        /// <param name="id">id фразы</param>
        /// <returns>Фраза с необходимым id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.DatabaseEntities.String>> GetStringById(int id)
        {
            Models.DatabaseEntities.String foundedString = await stringRepository.GetByIDAsync(id);

            if (foundedString == null)
            {
                return BadRequest("String not found");
            }

            return Ok(foundedString);
        }

    }
}
