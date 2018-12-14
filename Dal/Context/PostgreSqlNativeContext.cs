using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL.Context
{
    public class PostgreSqlNativeContext
    {
        private static PostgreSqlNativeContext instance;
        private static object obj = new object();

        private string connectionString = "User ID=postgres;Password=post123;Host=10.145.251.49;Port=5432;Database=localizationservice;Pooling=true;";

        private PostgreSqlNativeContext() { }

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
                    Console.WriteLine(exception.ErrorCode);

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
