using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class ProjectLocaleController : ControllerBase
    {

        //private readonly LocalizationProjectsLocalesRepository _localizationProjectsLocalesRepository;
        //private readonly UserActionRepository _userActionRepository;

        //public ProjectLocaleController()
        //{
        //    _localizationProjectRepository = new LocalizationProjectRepository(Settings.GetStringDB());
        //    _userActionRepository = new UserActionRepository(Settings.GetStringDB());
        //}



    }
}
