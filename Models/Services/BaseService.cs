using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Logs;

namespace Models.Services
{

    /// <summary>
    /// Базовый сервис
    /// </summary>
    public class BaseService
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
            WriteLn("ERROR: " + str);
            _loggerError.WriteLn(str, err);
            return str.ToString();
        }
    }
}
