using System;
using Utilities.Logs;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");



            LogTools lt = new LogTools();
            lt.WriteDebug("--123--");

        }
    }
}
