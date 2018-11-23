using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Models.Logs;

namespace Localization.Controllers
{
    /**
     * Класс, иллюстрирующий возможность получения данных в Angular из VS
     */
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private LogTools lt;

        public SampleDataController()
        {
            lt = new LogTools();
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            lt.WriteDebug("WeatherForecasts-->");

            WeatherForecast wf = new WeatherForecast();

            lt.WriteDebug("WeatherForecasts--<");
            return wf.getData();
        }
    }
}
