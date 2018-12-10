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
using System.Text;
using Utilities.Logs;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossaryRepository : IRepository<Glossary>
    {

        private readonly PostgreSqlNativeContext _context;

        private readonly IRepository<Models.DatabaseEntities.String> _stringsRepository;

        private readonly PostgresCompiler _compiler = new PostgresCompiler();

        private readonly LogTools _logger = new LogTools();

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
                var selectAllGlossariesSql = "SELECT * FROM \"Glossaries\"";
                this._logger.WriteDebug($"Query {selectAllGlossariesSql}");
                var glossaries = dbConnection.Query<Glossary>(selectAllGlossariesSql);
                dbConnection.Close();
                return glossaries;
            }
        }

        public Glossary GetByID(int id)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var getGlossaryByIdSql = "SELECT * FROM \"Glossaries\" WHERE \"ID\" = @GlossaryId LIMIT 1";
                var getGlossaryByIdParam = new { GlossaryId = id };
                this._logger.WriteDebug($"Query {getGlossaryByIdSql}, param: {getGlossaryByIdParam}");
                var glossary = dbConnection.QueryFirst<Glossary>(
                    sql: getGlossaryByIdSql,
                    param: getGlossaryByIdParam);
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
                var updateGlossarySql =
                    "UPDATE \"Glossaries\" SET \"Name\"=@Name, \"Description\"=@Description, \"ID_File\"=@ID_File " +
                    "WHERE \"ID\"=@ID";
                var updateGlossaryParam = item;
                this._logger.WriteDebug($"Query {updateGlossarySql}, param: {updateGlossaryParam}");
                dbConnection.Execute(
                    sql: updateGlossarySql,
                    param: updateGlossaryParam);
                dbConnection.Close();
            }
        }

        public void DeleteTerm(int glossaryId, int termId)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var deleteGlossaryStingAssotiationSql =
                    "DELETE FROM \"GlossariesStrings\" " +
                    "WHERE \"ID_Glossary\" = @GlossaryId AND \"ID_String\" = @TermId";
                var deleteGlossaryStingAssotiationParam = new { GlossaryId = glossaryId, TermId = termId };
                this._logger.WriteDebug($"Query {deleteGlossaryStingAssotiationSql}, param: {deleteGlossaryStingAssotiationParam}");
                dbConnection
                    .Execute(
                        sql: deleteGlossaryStingAssotiationSql,
                        param: deleteGlossaryStingAssotiationParam);

                var deleteStingSql = "DELETE FROM \"TranslationSubstrings\" WHERE \"ID\" = @TermId";
                var deleteStingParam = new { TermId = termId };
                this._logger.WriteDebug($"Query {deleteStingSql}, param: {deleteStingParam}");
                dbConnection
                    .Execute(
                        sql: deleteStingSql,
                        param: deleteStingParam);
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
                var insertNewStingSql =
                    "INSERT INTO \"TranslationSubstrings\" " +
                    "(" +
                    "\"SubstringToTranslate\", " +
                    "\"Description\", " +
                    "\"Context\", " +
                    "\"TranslationMaxLength\", " +
                    "\"ID_FileOwner\", " +
                    "\"Value\", " +
                    "\"PositionInText\"" +
                    ") VALUES " +
                    "(" +
                    "@SubstringToTranslate, " +
                    "@Description, " +
                    "@Context, " +
                    "@TranslationMaxLength, " +
                    "@ID_FileOwner, " +
                    "@Value, " +
                    "@PositionInText" +
                    ") " +
                    "RETURNING \"ID\"";
                var insertNewStingParam = newTerm;
                this._logger.WriteDebug($"Query {insertNewStingSql}, param: {insertNewStingParam}");
                var idOfNewTerm = dbConnection
                    .ExecuteScalar<int>(
                        sql: insertNewStingSql,
                        param: insertNewStingParam);

                var instertGlossaryStringAssotiationSql =
                    "INSERT INTO \"GlossariesStrings\" (\"ID_Glossary\", \"ID_String\") VALUES (@GlossaryId, @StringId)";
                var instertGlossaryStringAssotiationParam = new { GlossaryId = glossaryId, StringId = idOfNewTerm };
                this._logger.WriteDebug($"Query {instertGlossaryStringAssotiationSql}, param: {instertGlossaryStringAssotiationParam}");
                dbConnection
                    .Execute(
                        sql: instertGlossaryStringAssotiationSql,
                        param: instertGlossaryStringAssotiationParam);
                dbConnection.Close();
            }
        }

        public void UpdateTerm(Models.DatabaseEntities.String updatedTerm)
        {
            //TODO: call this._stringsRepository to do this
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var updateTermSql =
                    "UPDATE \"TranslationSubstrings\" SET " +
                    "\"SubstringToTranslate\"=@SubstringToTranslate, " +
                    "\"Description\"=@Description, " +
                    "\"Context\"=@Context, " +
                    "\"TranslationMaxLength\"=@TranslationMaxLength, " +
                    "\"ID_FileOwner\"=@ID_FileOwner, " +
                    "\"Value\"=@Value, " +
                    "\"PositionInText\"=@PositionInText " +
                    "WHERE \"ID\"=@ID";
                var updateTermParam = updatedTerm;
                this._logger.WriteDebug($"Query {updateTermSql}, param: {updateTermParam}");
                dbConnection.Execute(
                    sql: updateTermSql,
                    param: updateTermParam);
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
                        .Select(x => x.ToLower())
                        .Distinct()
                        .Where(x => GlossaryRepository.TermsSortColumnNamesMapping.ContainsKey(x))
                        .Select(x => GlossaryRepository.TermsSortColumnNamesMapping[x])
                        .ToArray();
                    if (columnNamesToSort.Any())
                        query = sortAscending? query.OrderBy(columnNamesToSort) : query.OrderByDesc(columnNamesToSort);
                }
                var getGlossaryTermsCompiledQuery = this._compiler.Compile(query);
                var getGlossaryTermsSql = getGlossaryTermsCompiledQuery.Sql;
                var getGlossaryTermsParam = getGlossaryTermsCompiledQuery.NamedBindings;
                this._logger.WriteDebug($"Query {getGlossaryTermsSql}, param: {this.DictionaryToString(getGlossaryTermsParam)}");
                var assotiatedTerms = dbConnection.Query<Models.DatabaseEntities.String>(
                    sql: getGlossaryTermsSql,
                    param: getGlossaryTermsParam
                    );
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
                var getGlossaryTermsCountCompiledQuery = this._compiler.Compile(query);
                var getGlossaryTermsCountSql = getGlossaryTermsCountCompiledQuery.Sql;
                var getGlossaryTermsCountParam = getGlossaryTermsCountCompiledQuery.NamedBindings;
                this._logger.WriteDebug($"Query {getGlossaryTermsCountSql}, param: {this.DictionaryToString(getGlossaryTermsCountParam)}");
                var assotiatedTerms = dbConnection.ExecuteScalar<int>(
                    sql: getGlossaryTermsCountSql,
                    param: getGlossaryTermsCountParam
                    );
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

        private string DictionaryToString(Dictionary<string, object> dictionary)
        {
            var stringBuilder =
                dictionary.SkipLast(1)
                .Aggregate(
                    seed: new StringBuilder("{ "),
                    func: (seed, pair) => seed.Append($"{pair.Key} = {pair.Value}, "));
            return dictionary.TakeLast(1)
                .Select(pair => stringBuilder.Append($"{pair.Key} = {pair.Value} }}"))
                .FirstOrDefault()?.ToString() ?? "null";
        }

    }
}
