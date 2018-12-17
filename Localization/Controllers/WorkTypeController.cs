using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;
using Microsoft.AspNetCore.Cors;

namespace Localization.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTypeController : ControllerBase
    {
        private readonly WorkTypeRepository workTypeRepository;

        public WorkTypeController()
        {
            workTypeRepository = new WorkTypeRepository();
        }

        /// <summary>
        /// GET api/WorkType/list       
        /// </summary>
        /// <returns>Список всех типов работы</returns>
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<WorkType>>> GetStrings()
        {
            var strings = await workTypeRepository.GetAllAsync();
            if (strings == null)
            {
                return BadRequest("Strings not found");
            }
            return Ok(strings);
        }

        /// <summary>
        /// // GET api/WorkType/:id
        /// </summary>
        /// <param name="id">id типа работы</param>
        /// <returns>Тип работы с необходимым id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkType>> GetWorkTypeById(int id)
        {
            WorkType typeOfWork = await workTypeRepository.GetByIDAsync(id);
            if (typeOfWork == null)
            {
                return BadRequest("Type of wprk not found");
            }
            return Ok(typeOfWork);
        }
    }
}
