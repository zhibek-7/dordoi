namespace Utilities
{
    /// <summary>
    /// Классы где будут храниться настройки.
    /// </summary>
    public class Settings : ISettings
    {
        private string _connectionString;

        // Настройки для email рассылки
        public const string EMailLogin = "qcoderitest@gmail.com";
        public const string EMailPassword = "NGY69Zrme4MFAT4";
        public const string EMailHost = "smtp.gmail.com";
        public const int EMailPort = 465; //587;
        //

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
