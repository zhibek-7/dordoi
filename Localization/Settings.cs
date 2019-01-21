using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Localization
{
    /// <summary>
    /// Классы где будут храниться настройки.
    /// </summary>
    public class Settings
    {

        public static string GetStringDB()
        {



            string connectionString = "Host=10.145.251.49;Port=5432;Database=localizationservice;Username=postgres;Password=post123;";
            return connectionString;
        }

    }
}
