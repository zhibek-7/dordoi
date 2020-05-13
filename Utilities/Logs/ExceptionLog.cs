using Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Logs
{
    public class ExceptionLog : LogTools
    {
        public ExceptionLog() : base("exceptionlog")
        {
            GetLog();
        }


        /// <summary>
        /// Получение класса для логирования с 
        /// </summary>
        /// <param name="logNameIn"></param>
        /// <returns></returns>
        public static ILogTools GetLog()
        {
            ILogTools lt = new LogTools("exceptionlog");
            return lt;
        }


    }
}
