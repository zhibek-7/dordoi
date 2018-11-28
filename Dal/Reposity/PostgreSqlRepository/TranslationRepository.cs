using System;
using System.Collections.Generic;
using System.Text;
using DAL.Context;
using System.Data;
using Dapper;
using Models.DatabaseEntities;
using System.Linq;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class TranslationRepository : IRepository<Translation>
    {
        private PostgreSqlNativeContext context;

        public TranslationRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(Translation item)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                dbConnection.Query<Translation>("INSERT INTO \"Translations\" (\"ID_String\", \"Translated\", \"Confirmed\", \"ID_User\", \"DateTime\") VALUES (@ID_String, @Translated, @Confirmed, @ID_User, @DateTime)", item);
                dbConnection.Close();
            }
        }

        public IEnumerable<Translation> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Translation> translations = dbConnection.Query<Translation>("SELECT * FROM \"Translations\"").ToList();
                dbConnection.Close();
                return translations;
            }
        }

        public Translation GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Translation item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Translation> GetAllTranslationsInStringByID(int idString)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Translation> translations = dbConnection.Query<Translation>("SELECT * FROM \"Translations\" WHERE \"ID_String\" = @Id ", new { Id = idString }).ToList();
                dbConnection.Close();
                return translations;
            }
        }
    }
}
