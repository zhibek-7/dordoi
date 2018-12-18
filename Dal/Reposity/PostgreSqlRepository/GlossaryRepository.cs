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
using System.Threading.Tasks;
using Utilities.Logs;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossaryRepository : IRepository<Glossary>
    {

        private readonly PostgreSqlNativeContext _context;

        private readonly IRepositoryAsync<Models.DatabaseEntities.TranslationSubstring> _stringsRepository;

        private readonly PostgresCompiler _compiler = new PostgresCompiler();

        private readonly LogTools _logger = new LogTools();

        public GlossaryRepository(IRepositoryAsync<Models.DatabaseEntities.TranslationSubstring> stringsRepository)
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
                this.LogQuery(selectAllGlossariesSql);
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
                this.LogQuery(getGlossaryByIdSql, getGlossaryByIdParam);
                var glossary = dbConnection.QueryFirst<Glossary>(
                    sql: getGlossaryByIdSql,
                    param: getGlossaryByIdParam);
                dbConnection.Close();
                return glossary;
            }
        }

        public bool Remove(int id)
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
                this.LogQuery(updateGlossarySql, updateGlossaryParam);
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
                this.LogQuery(deleteGlossaryStingAssotiationSql, deleteGlossaryStingAssotiationParam);
                dbConnection
                    .Execute(
                        sql: deleteGlossaryStingAssotiationSql,
                        param: deleteGlossaryStingAssotiationParam);

                var deleteStingSql = "DELETE FROM \"TranslationSubstrings\" WHERE \"ID\" = @TermId";
                var deleteStingParam = new { TermId = termId };
                this.LogQuery(deleteStingSql, deleteStingParam);
                dbConnection
                    .Execute(
                        sql: deleteStingSql,
                        param: deleteStingParam);
                dbConnection.Close();
            }
        }

        public void AddNewTerm(int glossaryId, Models.DatabaseEntities.TranslationSubstring newTerm, int? partOfSpeechId)
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
                this.LogQuery(insertNewStingSql, insertNewStingParam);
                var idOfNewTerm = dbConnection
                    .ExecuteScalar<int>(
                        sql: insertNewStingSql,
                        param: insertNewStingParam);

                var instertGlossaryStringAssotiationSql =
                    "INSERT INTO \"GlossariesStrings\" (\"ID_Glossary\", \"ID_String\",\"ID_PartOfSpeech\") VALUES (@GlossaryId, @StringId, @PartsOfSpeechId)";
                var instertGlossaryStringAssotiationParam = new { GlossaryId = glossaryId, StringId = idOfNewTerm, PartsOfSpeechId = partOfSpeechId };
                this.LogQuery(instertGlossaryStringAssotiationSql, instertGlossaryStringAssotiationParam);
                dbConnection
                    .Execute(
                        sql: instertGlossaryStringAssotiationSql,
                        param: instertGlossaryStringAssotiationParam);
                dbConnection.Close();
            }
        }

        public void UpdateTerm(int glossaryId, Models.DatabaseEntities.TranslationSubstring updatedTerm, int? partOfSpeechId)
        {
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
                this.LogQuery(updateTermSql, updateTermParam);
                dbConnection.Execute(
                    sql: updateTermSql,
                    param: updateTermParam);

                var updateTermPartOfSpeechIdSql =
                    "UPDATE \"GlossariesStrings\" SET " +
                    "\"ID_PartOfSpeech\"=@PartOfSpeechId " +
                    "WHERE \"ID_String\"=@StringId " +
                    "AND \"ID_Glossary\"=@GlossaryId";
                var updateTermPartOfSpeechIdParam = new { GlossaryId = glossaryId, StringId = updatedTerm.ID, PartOfSpeechId = partOfSpeechId };
                this.LogQuery(updateTermPartOfSpeechIdSql, updateTermPartOfSpeechIdParam);
                dbConnection.Execute(
                    sql: updateTermPartOfSpeechIdSql,
                    param: updateTermPartOfSpeechIdParam);
                dbConnection.Close();
            }
        }

        private static readonly Dictionary<string, string> TermsSortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "TranslationSubstrings.ID" },
            { "substringtotranslate", "TranslationSubstrings.SubstringToTranslate" },
            { "description", "TranslationSubstrings.Description" },
            { "context", "TranslationSubstrings.Context" },
            { "translationmaxlength", "TranslationSubstrings.TranslationMaxLength" },
            { "id_fileowner", "TranslationSubstrings.ID_FileOwner" },
            { "value", "TranslationSubstrings.Value" },
            { "positionintext", "TranslationSubstrings.PositionInText" },
        };

        public IEnumerable<Models.Glossaries.Term> GetAssotiatedTermsByGlossaryId(
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
                this.LogQuery(getGlossaryTermsSql, this.DictionaryToString(getGlossaryTermsParam));
                var assotiatedTerms = dbConnection.Query<Models.Glossaries.Term>(
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
                var query = this.GetAssotiatedTermsQuery(dbConnection, glossaryId, termPart).AsCount();
                var getGlossaryTermsCountCompiledQuery = this._compiler.Compile(query);
                var getGlossaryTermsCountSql = getGlossaryTermsCountCompiledQuery.Sql;
                var getGlossaryTermsCountParam = getGlossaryTermsCountCompiledQuery.NamedBindings;
                this.LogQuery(getGlossaryTermsCountSql, this.DictionaryToString(getGlossaryTermsCountParam));
                var assotiatedTermsCount = dbConnection.ExecuteScalar<int>(
                    sql: getGlossaryTermsCountSql,
                    param: getGlossaryTermsCountParam
                    );
                dbConnection.Close();
                return assotiatedTermsCount;
            }
        }

        private Query GetAssotiatedTermsQuery(IDbConnection dbConnection,int glossaryId, string termPart)
        {
            var query =
                new XQuery(dbConnection, this._compiler)
                    .From("GlossariesStrings")
                    .LeftJoin("TranslationSubstrings", "TranslationSubstrings.ID", "GlossariesStrings.ID_String")
                    .Where("GlossariesStrings.ID_Glossary", glossaryId)
                    .Select(
                        "TranslationSubstrings.ID",
                        "TranslationSubstrings.SubstringToTranslate",
                        "TranslationSubstrings.Description",
                        "TranslationSubstrings.Context",
                        "TranslationSubstrings.TranslationMaxLength",
                        "TranslationSubstrings.ID_FileOwner",
                        "TranslationSubstrings.Value",
                        "TranslationSubstrings.PositionInText",
                        "GlossariesStrings.ID_PartOfSpeech as PartOfSpeechId")
                    .Select(
                        new Query("Translations")
                            .SelectRaw("COUNT(\"Translated\") = 0")
                            .Where("Translated", "<>", "''")
                            .WhereRaw("\"ID_String\"=\"TranslationSubstrings\".\"ID\""),
                        "IsEditable");
            if (!string.IsNullOrEmpty(termPart))
            {
                var patternString = $"%{termPart}%";
                query = query.WhereLike("TranslationSubstrings.Value", patternString);
            }
            return query;
        }

        public Locale GetLocaleById(int glossaryId)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var getGlossaryLocaleSql = "SELECT * FROM \"Locales\" WHERE \"ID\" IN " +
                    "(SELECT \"ID_SourceLocale\" FROM \"LocalizationProjects\" WHERE \"ID\" IN " +
                    "(SELECT \"ID_LocalizationProject\" FROM \"LocalizationProjectsGlossaries\" WHERE \"ID_Glossary\"=@GlossaryId))";
                var getGlossaryLocaleParam = new { GlossaryId = glossaryId };
                this.LogQuery(getGlossaryLocaleSql, getGlossaryLocaleParam);
                var locale = dbConnection
                    .QueryFirst<Locale>(
                        sql: getGlossaryLocaleSql,
                        param: getGlossaryLocaleParam);
                dbConnection.Close();
                return locale;
            }
        }

        public IEnumerable<Locale> GetTranslationLocalesForTerm(int glossaryId, int termId)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var getTranslationLocalesForTermQuery =
                    new Query("Locales")
                    .WhereIn("ID",
                        new Query("TranslationsubStringsLocales")
                        .Select("Id_Locales")
                        .Where("Id_TranslationSubStrings", termId));
                var getTranslationLocalesForTermCompiledQuery = this._compiler.Compile(getTranslationLocalesForTermQuery);
                var getTranslationLocalesForTermSql = getTranslationLocalesForTermCompiledQuery.Sql;
                var getTranslationLocalesForTermParam = getTranslationLocalesForTermCompiledQuery.NamedBindings;
                this.LogQuery(getTranslationLocalesForTermSql, this.DictionaryToString(getTranslationLocalesForTermParam));
                var translationLocalesForTerm = dbConnection.Query<Locale>(
                    sql: getTranslationLocalesForTermSql,
                    param: getTranslationLocalesForTermParam);
                dbConnection.Close();
                return translationLocalesForTerm;
            }
        }

        public IEnumerable<Locale> GetTranslationLocales(int glossaryId)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var getTranslationLocalesQuery =
                    new Query("Locales")
                    .WhereIn("ID",
                        new Query("GlossariesLocales")
                        .Select("ID_Locale")
                        .Where("ID_Glossary", glossaryId));
                var getTranslationLocalesmCompiledQuery = this._compiler.Compile(getTranslationLocalesQuery);
                var getTranslationLocalesmSql = getTranslationLocalesmCompiledQuery.Sql;
                var getTranslationLocalesmParam = getTranslationLocalesmCompiledQuery.NamedBindings;
                this.LogQuery(getTranslationLocalesmSql, this.DictionaryToString(getTranslationLocalesmParam));
                var translationLocalesForTerm = dbConnection.Query<Locale>(
                    sql: getTranslationLocalesmSql,
                    param: getTranslationLocalesmParam);
                dbConnection.Close();
                return translationLocalesForTerm;
            }
        }

        public IEnumerable<Locale> DeleteTranslationLocalesForTerm(int termId)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                var deleteTranslationLocalesForTermQuery =
                    new Query("TranslationsubStringsLocales")
                    .Where("Id_TranslationSubStrings", termId)
                    .AsDelete();
                var deleteTranslationLocalesForTermCompiledQuery = this._compiler.Compile(deleteTranslationLocalesForTermQuery);
                var deleteTranslationLocalesForTermSql = deleteTranslationLocalesForTermCompiledQuery.Sql;
                var deleteTranslationLocalesForTermParam = deleteTranslationLocalesForTermCompiledQuery.NamedBindings;
                this.LogQuery(deleteTranslationLocalesForTermSql, this.DictionaryToString(deleteTranslationLocalesForTermParam));
                var translationLocalesForTerm = dbConnection.Query<Locale>(
                    sql: deleteTranslationLocalesForTermSql,
                    param: deleteTranslationLocalesForTermParam);
                dbConnection.Close();
                return translationLocalesForTerm;
            }
        }

        public void SetTranslationLocalesForTerm(int termId, IEnumerable<int> localesIds)
        {
            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();
                foreach(var localeId in localesIds)
                {
                    var getTranslationLocalesForTermSql =
                        "INSERT INTO \"TranslationsubStringsLocales\" " +
                        "(" +
                        "\"Id_TranslationSubStrings\", " +
                        "\"Id_Locales\"" +
                        ") VALUES " +
                        "(" +
                        "@Id_TranslationSubStrings, " +
                        "@Id_Locales" +
                        ")";
                    var getTranslationLocalesForTermParam = new { Id_TranslationSubStrings = termId, Id_Locales = localeId };
                    this.LogQuery(getTranslationLocalesForTermSql, getTranslationLocalesForTermParam);
                    dbConnection.Execute(
                        sql: getTranslationLocalesForTermSql,
                        param: getTranslationLocalesForTermParam);
                }
                dbConnection.Close();
            }
        }

        private void LogQuery(string sql)
        {
            this._logger.WriteDebug($"Query {sql}");
        }

        private void LogQuery(string sql, object param)
        {
            this._logger.WriteDebug($"Query {sql}, param: {param}");
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
