using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class PartOfSpeechRepository : IRepository<PartOfSpeech>
    {

        private readonly PostgreSqlNativeContext _context = PostgreSqlNativeContext.getInstance();

        public void Add(PartOfSpeech item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PartOfSpeech> GetAll()
        {
            throw new NotImplementedException();
        }

        public PartOfSpeech GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PartOfSpeech item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PartOfSpeech> GetByGlossaryId(int glossaryId)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var partsOfSpeechForLocale = dbConnection
                    .Query<PartOfSpeech>(
                        sql: "SELECT * FROM \"PartsOfSpeech\" WHERE \"LocaleID\" IN " +
                        "(SELECT \"ID_Locale\" FROM \"GlossariesLocales\" WHERE \"ID_Glossary\"=@GlossaryId)",
                        param: new
                        {
                            GlossaryId = glossaryId
                        });
                dbConnection.Close();
                return partsOfSpeechForLocale;
            }
        }

    }
}
