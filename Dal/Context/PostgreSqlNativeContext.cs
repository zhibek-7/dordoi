﻿using Npgsql;
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

        private string connectionString = "User ID=postgres;Password=post123;Host=coderlink02.taximaxim.local;Port=5432;Database=localizationservice;Pooling=true;";

        private PostgreSqlNativeContext() { }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public static PostgreSqlNativeContext getInstance()
        {
            lock(obj)
            {
                if(instance == null)
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