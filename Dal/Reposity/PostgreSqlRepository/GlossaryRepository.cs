using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossaryRepository : IRepository<Glossary>
    {

        private readonly PostgreSqlNativeContext _context;

        private readonly IRepository<Models.DatabaseEntities.String> _stringsRepository;

        private readonly PostgresCompiler _compiler = new PostgresCompiler();

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
                        "\"PositionInText\") VALUES " +
                        "(@SubstringToTranslate, @Description, @Context, @TranslationMaxLength, " +
                        "@ID_FileOwner, @Value, @PositionInText) " +
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
                    "\"PositionInText\"=@PositionInText " +
                    "WHERE \"ID\"=@ID",
                    param: updatedTerm);
                dbConnection.Close();
            }
        }

        private static readonly Dictionary<string, string> TermsSortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "ID" },
            { "substringtotranslate", "SubstringToTranslate" },
            { "description", "Description" },
            { "context", "Context" },
            { "translationmaxlength", "TranslationMaxLength" },
            { "id_fileowner", "ID_FileOwner" },
            { "value", "Value" },
            { "positionintext", "PositionInText" },
        };

        public IEnumerable<Models.DatabaseEntities.String> GetAssotiatedTermsByGlossaryId(
            int glossaryId,
            int pageSize,
            int pageNumber,
            string termPart = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            if (sortBy == null)
                sortBy = new[] { "id" };

            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var query = this.GetAssotiatedTermsQuery(dbConnection, glossaryId, termPart);
                if (pageSize > 0 && pageNumber > 0)
                {
                    query = query.Skip(pageSize * (pageNumber - 1));
                }
                if (pageSize > 0)
                {
                    query = query.Take(pageSize);
                }
                if (sortBy.Any())
                {
                    var columnNamesToSort = sortBy
                        .Distinct()
                        .Where(x => GlossaryRepository.TermsSortColumnNamesMapping.ContainsKey(x))
                        .Select(x => GlossaryRepository.TermsSortColumnNamesMapping[x.ToLower()])
                        .ToArray();
                    if (columnNamesToSort.Any())
                        query = sortAscending? query.OrderBy(columnNamesToSort) : query.OrderByDesc(columnNamesToSort);
                }
                var assotiatedTerms = query.Get<Models.DatabaseEntities.String>();
                dbConnection.Close();
                return assotiatedTerms;
            }
        }

        public int GetAssotiatedTermsCount(int glossaryId, string termPart)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var query = this.GetAssotiatedTermsQuery(dbConnection, glossaryId, termPart);
                var assotiatedTermsCount = query.Count<int>();
                dbConnection.Close();
                return assotiatedTermsCount;
            }
        }

        private Query GetAssotiatedTermsQuery(IDbConnection dbConnection,int glossaryId, string termPart)
        {
            var query =
                new XQuery(dbConnection, this._compiler)
                    .From("TranslationSubstrings")
                    .WhereIn("ID",
                        new Query("GlossariesStrings")
                            .Select("ID_String")
                            .Where("ID_Glossary", glossaryId));
            if (!string.IsNullOrEmpty(termPart))
            {
                var patternString = $"%{termPart}%";
                query = query.WhereLike("Value", patternString);
            }
            return query;
        }

    }
}
