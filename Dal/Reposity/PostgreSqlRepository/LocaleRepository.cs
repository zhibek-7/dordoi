using System;
using System.Collections.Generic;
using Models.DatabaseEntities;
using Dapper;
using System.Data;
using System.Linq;
using DAL.Context;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocaleRepository : IRepository<Locale>
    {
        private PostgreSqlNativeContext context;

        public LocaleRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(Locale locale)
        {
            throw new NotImplementedException();
        }

        public Locale GetByID(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Locale> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Locale> users = dbConnection.Query<Locale>("SELECT * FROM \"Locales\"").ToList();
                dbConnection.Close();
                return users;
            }
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Locale user)
        {
            throw new NotImplementedException();
        }
    }
}

