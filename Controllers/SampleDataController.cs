using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Utilities.Logs;

namespace Localization.Controllers
{
    /**
     * �����, �������������� ����������� ��������� ������ � Angular �� VS
     */
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private LogTools lt;
        private ExceptionLog ltErro;

        public SampleDataController()
        {
            lt = new LogTools();
            ltErro = new ExceptionLog();
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            lt.WriteDebug("WeatherForecasts-->");

            try
            {
                WeatherForecast wf = new WeatherForecast();


                lt.WriteDebug("WeatherForecasts--<");
                return wf.getData();

            }
            catch (Exception exc)
            {
                ltErro.WriteExceprion("������ ��� �������� ������ ", exc);
            }

            return null;
        }
    }
}
