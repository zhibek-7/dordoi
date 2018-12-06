using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using Dapper;
using System.Linq;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class StringRepository : IRepository<Models.DatabaseEntities.String>
    {
        private PostgreSqlNativeContext context;

        public StringRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(Models.DatabaseEntities.String item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.DatabaseEntities.String> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Models.DatabaseEntities.String> strings = dbConnection.Query<Models.DatabaseEntities.String>("SELECT * FROM \"TranslationSubstrings\"").ToList();
                dbConnection.Close();
                return strings;
            }
        }

        public Models.DatabaseEntities.String GetByID(int id)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                Models.DatabaseEntities.String foundedString = dbConnection.Query<Models.DatabaseEntities.String>("SELECT * FROM \"Strings\" WHERE \"ID\" = @Id", new { Id = id }).FirstOrDefault();
                dbConnection.Close();
                return foundedString;
            }
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Models.DatabaseEntities.String item)
        {
            throw new NotImplementedException();
        }
    }
}
