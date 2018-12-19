using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Utilities.Logs;

namespace DAL.Context
{
    public class PostgreSqlNativeContext
    {
        private static PostgreSqlNativeContext instance;
        private static object obj = new object();

        private string connectionString = "User ID=postgres;Password=post123;Host=10.145.251.49;Port=5432;Database=localizationservice;Pooling=true;";

        private PostgreSqlNativeContext() { }


        internal string ConnectionString
        {

            get
            {
                return connectionString;
            }
        }
        internal IDbConnection Connection
        {
            get
            {
                try
                {
                    return new NpgsqlConnection(connectionString);
                }
                catch (NpgsqlException exception)
                {


                    //внесение записи в журанал логирования
                    LogTools.GetLog().WriteLn("Ошибка в Connection ", exception);

                    return null;
                }
            }
        }

        public static PostgreSqlNativeContext getInstance()
        {
            lock (obj)
            {
                if (instance == null)
                {
                    instance = new PostgreSqlNativeContext();
                }
            }

            return instance;
        }

        public void SetConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }

    }
}
