using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using Models.DatabaseEntities;
using Utilities.Logs;

namespace Test
{
    class Program
    {



        static void Main(string[] args)
        {
            SettingJson sj = new SettingJson();
            sj.WriteSettings();


        }
    }
}
