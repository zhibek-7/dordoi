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
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var glossary = dbConnection.QueryFirst<Glossary>("SELECT * FROM \"Glossaries\" WHERE \"ID\" = @GlossaryId", new { GlossaryId = id });
                dbConnection.Close();
                return glossary;
            }
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Glossary item)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                dbConnection.Execute(
                    sql: "UPDATE \"Glossaries\" SET \"Name\"=@Name, \"Description\"=@Description, \"ID_File\"=@ID_File " +
                    "WHERE \"ID\"=@ID",
                    param: item);
                dbConnection.Close();
            }
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

        public void AddNewTerm(int glossaryId, Models.DatabaseEntities.String newTerm)
        {
            var glossary = this.GetByID(id: glossaryId);
            newTerm.ID_FileOwner = glossary.ID_File;
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var idOfNewTerm = dbConnection
                    .ExecuteScalar<int>(
                        sql: "INSERT INTO \"Strings\" " +
                        "(\"SubstringToTranslate\", \"Description\", \"Context\", " +
                        "\"TranslationMaxLength\", \"ID_FileOwner\", \"PositionInFile\", " +
                        "\"OriginalString\", \"HasTranslationSubstring\", \"TranslationSubstring\", " +
                        "\"TranslationSubstringPositionInLine\") VALUES " +
                        "(@SubstringToTranslate, @Description, @Context, @TranslationMaxLength, " +
                        "@ID_FileOwner, @PositionInFile, @OriginalString, @HasTranslationSubstring, " +
                        "@TranslationSubstring, @TranslationSubstringPositionInLine) " +
                        "RETURNING \"ID\"",
                        param: newTerm);
                dbConnection
                    .Execute(
                        sql: "INSERT INTO \"GlossariesStrings\" (\"ID_Glossary\", \"ID_String\") VALUES (@GlossaryId, @StringId)",
                        param: new { GlossaryId = glossaryId, StringId = idOfNewTerm });
                dbConnection.Close();
            }
        }

    }
}
