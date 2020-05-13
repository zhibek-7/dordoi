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

        public string WriteTextLn(Object str)
        {
            _logger.WriteLn(str);
            return str.ToString();
        }

        public string WriteTextLn(Exception err)
        {
            WriteTextLn("ERROR:" + err.Message);
            _loggerError.WriteLn(err.Message, err);
            return err.Message.ToString();
        }
    }
}
