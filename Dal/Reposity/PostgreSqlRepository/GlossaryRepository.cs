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

        private readonly IRepository<Models.DatabaseEntities.String> _stringsRepository;

        public GlossaryRepository(IRepository<Models.DatabaseEntities.String> stringsRepository)
        {
            this._context = PostgreSqlNativeContext.getInstance();
            this._stringsRepository = stringsRepository;
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
                var glossary = dbConnection.QueryFirst<Glossary>(
                    sql: "SELECT * FROM \"Glossaries\" WHERE \"ID\" = @GlossaryId LIMIT 1",
                    param: new { GlossaryId = id });
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

        public void DeleteTerm(int glossaryId, int termId)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                dbConnection
                    .Execute(
                        sql: "DELETE FROM \"GlossariesStrings\" WHERE \"ID_Glossary\" = @GlossaryId AND \"ID_String\" = @TermId",
                        param: new { GlossaryId = glossaryId, TermId = termId });
                dbConnection
                    .Execute(
                        sql: "DELETE FROM \"TranslationSubstrings\" WHERE \"ID\" = @TermId",
                        param: new { TermId = termId });
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
                            sql: "SELECT * FROM \"TranslationSubstrings\" "
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
                        sql: "INSERT INTO \"TranslationSubstrings\" " +
                        "(\"SubstringToTranslate\", \"Description\", \"Context\", " +
                        "\"TranslationMaxLength\", \"ID_FileOwner\", \"Value\", " +
                        "\"PositionInFullText\") VALUES " +
                        "(@SubstringToTranslate, @Description, @Context, @TranslationMaxLength, " +
                        "@ID_FileOwner, @Value, @PositionInFullText) " +
                        "RETURNING \"ID\"",
                        param: newTerm);
                dbConnection
                    .Execute(
                        sql: "INSERT INTO \"GlossariesStrings\" (\"ID_Glossary\", \"ID_String\") VALUES (@GlossaryId, @StringId)",
                        param: new { GlossaryId = glossaryId, StringId = idOfNewTerm });
                dbConnection.Close();
            }
        }

        public void UpdateTerm(Models.DatabaseEntities.String updatedTerm)
        {
            //TODO: call this._stringsRepository to do this
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                dbConnection.Execute(
                    sql: "UPDATE \"TranslationSubstrings\" SET " +
                    "\"SubstringToTranslate\"=@SubstringToTranslate, \"Description\"=@Description, \"Context\"=@Context, " +
                    "\"TranslationMaxLength\"=@TranslationMaxLength, \"ID_FileOwner\"=@ID_FileOwner, \"Value\"=@Value, " +
                    "\"PositionInFullText\"=@PositionInFullText " +
                    "WHERE \"ID\"=@ID",
                    param: updatedTerm);
                dbConnection.Close();
            }
        }

        public IEnumerable<Models.DatabaseEntities.String> FindAssotiatedTermsByPart(int glossaryId, string termPart)
        {
            var patternString = $"%{termPart}%";
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var foundedTerms = dbConnection
                    .Query<Models.DatabaseEntities.String>(
                        sql: "SELECT * FROM \"TranslationSubstrings\" " +
                        "WHERE \"ID\" IN (SELECT \"ID_String\" FROM \"GlossariesStrings\" WHERE \"ID_Glossary\" = @GlossaryId) " +
                        "AND \"Value\" LIKE @TermPattern",
                        param: new
                        {
                            GlossaryId = glossaryId,
                            TermPattern = new DbString
                            {
                                Value = patternString,
                                IsFixedLength = false,
                                Length = patternString.Length,
                                IsAnsi = false
                            }
                        });
                dbConnection.Close();
                return foundedTerms;
            }
        }

    }
}
