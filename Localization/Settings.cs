using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
namespace Localization
{
    /// <summary>
    /// Классы где будут храниться настройки.
    /// </summary>
    public class Settings : ISetttings
    {
        private string _connectionString;

        public static string GetStringDB()
        {
            //TODO нужно доработать метод
            var connectionString = "Host=10.145.251.49;Port=5432;Database=localizationsrv;Username=postgres;Password=post123;";
            //var connectionString = "Host=localhost;Port=5432;Database=localizationsrv;Username=postgres;Password=post123;";

            return connectionString;
        }

        public Settings(string connectionString)
        {

            this._connectionString = connectionString;

        }

    }
}
