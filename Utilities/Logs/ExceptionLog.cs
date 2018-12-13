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
    }
}
