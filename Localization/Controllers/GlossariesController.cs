using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossariesController : ControllerBase
    {

        private readonly GlossariesService _glossariesService;

        public GlossariesController(GlossariesService glossariesService)
        {
            this._glossariesService = glossariesService;
        }

        [HttpGet]
        public async Task<IEnumerable<Glossaries>> GetAllAsync()
        {
            return await this._glossariesService.GetAllAsync();
        }


        [HttpGet]
        [Route("ToDTO")]
        public async Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync()
        {
            return await this._glossariesService.GetAllToDTOAsync();
        }
    }
}
