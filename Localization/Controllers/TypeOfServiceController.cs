using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class TypeOfServiceController : ControllerBase
    {
        private readonly ITypeOfServiceRepository _typeOfServiceRepository;

        public TypeOfServiceController(ITypeOfServiceRepository typeOfServiceRepository)
        {
            _typeOfServiceRepository = typeOfServiceRepository;
        }

        /// <summary>
        /// Возвращает список тип услуг, содержащий только идентификатор и наименование.
        /// Для выборки, например checkbox.
        /// </summary>
        /// <returns>{id, name_text}</returns>
        [Authorize]
        [HttpPost("forSelect")]
        public async Task<IEnumerable<TypeOfServiceForSelectDTO>> GetAllForSelectAsync()
        {
            return await _typeOfServiceRepository.GetAllForSelectAsync();
        }
    }
}
