using System.Collections.Generic;
using System.Linq;
using Utilities.Parser;

namespace Utilities
{
    /// <summary>
    /// Классы где будут храниться настройки.
    /// </summary>
    public class Settings : ISettings
    {
        private static Settings st;

        private Dictionary<string, string> list;
        private string _connectionString;

        // Настройки для email рассылки
        //public string EMailLogin { get list ["Email_Login"]; } = list["Email_Login"];// "qcoderitest@gmail.com";
        // public string EMailPassword { get; } = list["Email_Password"];// "NGY69Zrme4MFAT4";
        // public string EMailHost { get; } = list["Email_Host"];// "smtp.gmail.com";
        // public string EMailPort { get; } = list["Email_Port"];// 465; //587;
        //

        public static Settings getSettings()
        {
            if (st == null)
            {
                st = new Settings();
            }

            return st;
        }


        public static string GetStringDB()
        {

            var connectionString = Settings.getSettings().GetString("ConnectionStrings_db_connection");
            //list = (new SettingJson()).WriteSettings();
            //var connectionString = list["ConnectionStrings_db_connection"];
            //"Host=10.145.251.49;Port=5432;Database=localizationsrv;Username=postgres;Password=post123;";
            //var connectionString = "Host=localhost;Port=5432;Database=localizationsrv;Username=postgres;Password=post123;";

            return connectionString;
        }


        public string GetString(string key)
        {
            string value = null;
            if (key != null && list.Keys.Contains(key))
            {
                value = list[key];
            }

            return value;
        }

        public Settings()
        {
            st = this;
            SettingJson sj = new SettingJson();

            if (sj != null)
            {
                list = sj.WriteSettings();
            }
        }

        public Settings(string connectionString) : this()
        {
            this._connectionString = connectionString;
        }

    }
}
