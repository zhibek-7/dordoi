using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilities.Logs;

namespace Localization.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogTools _logger = new LogTools();
        protected readonly ILogTools _loggerError = new ExceptionLog();

        public string WriteLn(Object str)
        {
            _logger.WriteLn(str);
            return str.ToString();
        }

        public string WriteLn(Object str, Exception err)
        {
            WriteLn("ERROR:" + str);
            _loggerError.WriteLn(str, err);
            return str.ToString();
        }
    }
}
