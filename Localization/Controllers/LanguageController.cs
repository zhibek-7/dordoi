using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly LocaleRepository _localeRepository;

        public LanguageController()
        {
            _localeRepository = new LocaleRepository();
        }

        [HttpGet]
        [Route("List")]
        public async Task<IEnumerable<Locale>> GetLocales()
        {
            return await _localeRepository.GetAllAsync();
        }

        [HttpGet("byProjectId/{projectId}")]
        public async Task<IEnumerable<Locale>> GetProjectLocales(int projectId)
        {
            return await _localeRepository.GetAllForProject(projectId);
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<IEnumerable<Locale>> GetUserLocales(int userId)
        {
            return await _localeRepository.GetByUserIdAsync(userId);
        }

    }
}
