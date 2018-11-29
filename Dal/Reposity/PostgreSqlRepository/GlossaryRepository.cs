using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using System;
using System.Collections.Generic;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossaryRepository : IRepository<Glossary>
    {

        private readonly PostgreSqlNativeContext _context;

        public GlossaryRepository()
        {
            this._context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(Glossary item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Glossary> GetAll()
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var glossaries = dbConnection.Query<Glossary>("SELECT * FROM \"Glossaries\"");
                dbConnection.Close();
                return glossaries;
            }
        }

        public Glossary GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Glossary item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.DatabaseEntities.String> GetAssotiatedTermsByGlossaryId(int id)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var terms =
                    dbConnection
                        .Query<Models.DatabaseEntities.String>(
                            sql: "SELECT * FROM \"Strings\" "
                            + "WHERE \"ID\" IN (SELECT \"ID_String\" FROM \"GlossariesStrings\" WHERE \"ID_Glossary\" = @GlossaryId)",
                            param: new { GlossaryId = id });
                dbConnection.Close();
                return terms;
            }
        }

    }
}
