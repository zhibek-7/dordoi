using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Utilities.Logs
{
    public interface ILogTools
    {
        void WriteLn(Object str);
        void WriteLn(Object str, Exception err);
    }
}
