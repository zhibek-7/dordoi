using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Models.DatabaseEntities.PartialEntities.Glossaries;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossaryRepository : BaseRepository, IGlossaryRepository
    {

        public GlossaryRepository(string connectionString) : base(connectionString)
        {
        }

        public Task<int> AddAsync(Glossary item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Glossary>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var selectAllGlossariesSql = "SELECT * FROM \"Glossaries\"";
                    this.LogQuery(selectAllGlossariesSql);
                    var glossaries = await dbConnection.QueryAsync<Glossary>(selectAllGlossariesSql);
                    return glossaries;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<Glossary> GetByIDAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var getGlossaryByIdSql = "SELECT * FROM \"Glossaries\" WHERE \"ID\" = @GlossaryId LIMIT 1";
                    var getGlossaryByIdParam = new { GlossaryId = id };
                    this.LogQuery(getGlossaryByIdSql, getGlossaryByIdParam);
                    var glossary = await dbConnection.QueryFirstAsync<Glossary>(
                        sql: getGlossaryByIdSql,
                        param: getGlossaryByIdParam);
                    return glossary;



                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<Glossary> GetByFileIdAsync(int fileId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sql = "SELECT * FROM \"Glossaries\" WHERE \"ID_File\" = @FileId LIMIT 1";
                    var param = new { FileId = fileId };
                    this.LogQuery(sql, param);
                    var glossary = await dbConnection.QueryFirstOrDefaultAsync<Glossary>(
                        sql: sql,
                        param: param);
                    return glossary;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetByFileIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetByFileIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Glossary item)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var updateGlossarySql =
                        "UPDATE \"Glossaries\" SET \"Name\"=@Name, \"Description\"=@Description, \"ID_File\"=@ID_File " +
                        "WHERE \"ID\"=@ID";
                    var updateGlossaryParam = item;
                    this.LogQuery(updateGlossarySql, updateGlossaryParam);
                    await dbConnection.ExecuteAsync(
                        sql: updateGlossarySql,
                        param: updateGlossaryParam);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.UpdateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;

            }
        }

        public async Task DeleteTermAsync(int glossaryId, int termId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var deleteGlossaryStingAssotiationSql =
                        "DELETE FROM \"GlossariesStrings\" " +
                        "WHERE \"ID_Glossary\" = @GlossaryId AND \"ID_String\" = @TermId";
                    var deleteGlossaryStingAssotiationParam = new { GlossaryId = glossaryId, TermId = termId };
                    this.LogQuery(deleteGlossaryStingAssotiationSql, deleteGlossaryStingAssotiationParam);
                    await dbConnection
                        .ExecuteAsync(
                            sql: deleteGlossaryStingAssotiationSql,
                            param: deleteGlossaryStingAssotiationParam);

                    var deleteStingSql = "DELETE FROM \"TranslationSubstrings\" WHERE \"ID\" = @TermId";
                    var deleteStingParam = new { TermId = termId };
                    this.LogQuery(deleteStingSql, deleteStingParam);
                    await dbConnection
                        .ExecuteAsync(
                            sql: deleteStingSql,
                            param: deleteStingParam);

                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.DeleteTermAsync)} {nameof(NpgsqlException)} ",
                      exception);
                //return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.DeleteTermAsync)} {nameof(Exception)} ",
                    exception);
                //return false;
            }

        }

        public async Task<int> AddNewTermAsync(int glossaryId, TranslationSubstring newTerm, int? partOfSpeechId)
        {
            var glossary = await this.GetByIDAsync(id: glossaryId);
            newTerm.ID_FileOwner = glossary.ID_File;
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
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
                    var idOfNewTerm = await dbConnection
                        .ExecuteScalarAsync<int>(
                            sql: insertNewStingSql,
                            param: insertNewStingParam);

                    var instertGlossaryStringAssotiationSql =
                        "INSERT INTO \"GlossariesStrings\" (\"ID_Glossary\", \"ID_String\",\"ID_PartOfSpeech\") VALUES (@GlossaryId, @StringId, @PartsOfSpeechId)";
                    var instertGlossaryStringAssotiationParam = new { GlossaryId = glossaryId, StringId = idOfNewTerm, PartsOfSpeechId = partOfSpeechId };
                    this.LogQuery(instertGlossaryStringAssotiationSql, instertGlossaryStringAssotiationParam);
                    await dbConnection
                        .ExecuteAsync(
                            sql: instertGlossaryStringAssotiationSql,
                            param: instertGlossaryStringAssotiationParam);
                    return idOfNewTerm;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.AddNewTermAsync)} {nameof(NpgsqlException)} ",
                      exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.AddNewTermAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        public async Task UpdateTermAsync(int glossaryId, TranslationSubstring updatedTerm, int? partOfSpeechId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
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
                    await dbConnection.ExecuteAsync(
                        sql: updateTermSql,
                        param: updateTermParam);

                    var updateTermPartOfSpeechIdSql =
                        "UPDATE \"GlossariesStrings\" SET " +
                        "\"ID_PartOfSpeech\"=@PartOfSpeechId " +
                        "WHERE \"ID_String\"=@StringId " +
                        "AND \"ID_Glossary\"=@GlossaryId";
                    var updateTermPartOfSpeechIdParam = new { GlossaryId = glossaryId, StringId = updatedTerm.ID, PartOfSpeechId = partOfSpeechId };
                    this.LogQuery(updateTermPartOfSpeechIdSql, updateTermPartOfSpeechIdParam);
                    await dbConnection.ExecuteAsync(
                        sql: updateTermPartOfSpeechIdSql,
                        param: updateTermPartOfSpeechIdParam);

                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.UpdateTermAsync)} {nameof(NpgsqlException)} ",
                      exception);
                //   return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.UpdateTermAsync)} {nameof(Exception)} ",
                    exception);
                //    return 0;
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

        public async Task<IEnumerable<Term>> GetAssotiatedTermsByGlossaryIdAsync(
            int glossaryId,
            int limit,
            int offset,
            string termPart = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            if (sortBy == null)
                sortBy = new[] { "id" };
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetAssotiatedTermsQuery(glossaryId, termPart);

                    query = this.ApplyPagination(
                        query: query,
                        offset: offset,
                        limit: limit);

                    query = this.ApplySorting(
                        query: query,
                        columnNamesMappings: GlossaryRepository.TermsSortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var getGlossaryTermsCompiledQuery = this._compiler.Compile(query);
                    this.LogQuery(getGlossaryTermsCompiledQuery);
                    var assotiatedTerms = await dbConnection.QueryAsync<Term>(
                        sql: getGlossaryTermsCompiledQuery.Sql,
                        param: getGlossaryTermsCompiledQuery.NamedBindings
                        );
                    return assotiatedTerms;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAssotiatedTermsByGlossaryIdAsync)} {nameof(NpgsqlException)} ",
                      exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAssotiatedTermsByGlossaryIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;

            }
        }

        public async Task<int> GetAssotiatedTermsCountAsync(int glossaryId, string termPart)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetAssotiatedTermsQuery(glossaryId, termPart).AsCount();
                    var getGlossaryTermsCountCompiledQuery = this._compiler.Compile(query);
                    this.LogQuery(getGlossaryTermsCountCompiledQuery);
                    var assotiatedTermsCount = await dbConnection.ExecuteScalarAsync<int>(
                        sql: getGlossaryTermsCountCompiledQuery.Sql,
                        param: getGlossaryTermsCountCompiledQuery.NamedBindings
                        );
                    return assotiatedTermsCount;

                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAssotiatedTermsCountAsync)} {nameof(NpgsqlException)} ",
                      exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAssotiatedTermsCountAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        private Query GetAssotiatedTermsQuery(int glossaryId, string termPart)
        {
            var query =
                new Query("GlossariesStrings")
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
                        new Query("TranslationsubStringsLocales")
                            .LeftJoin("Translations", join =>
                                join.On("Translations.ID_String", "TranslationsubStringsLocales.Id_TranslationSubStrings")
                                    .On("Translations.ID_Locale", "TranslationsubStringsLocales.Id_Locales"))
                            .SelectRaw("COUNT(\"Translations\".\"Translated\") = 0")
                            .Where("Translations.Translated", "<>", "''")
                            .WhereRaw("\"TranslationsubStringsLocales\".\"Id_TranslationSubStrings\"=\"TranslationSubstrings\".\"ID\""),
                        "IsEditable");
            if (!string.IsNullOrEmpty(termPart))
            {
                var patternString = $"%{termPart}%";
                query = query.WhereLike("TranslationSubstrings.Value", patternString);
            }
            return query;
        }

        public async Task<Locale> GetLocaleByIdAsync(int glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var getGlossaryLocaleSql = "SELECT * FROM \"Locales\" WHERE \"ID\" IN " +
                        "(SELECT \"ID_SourceLocale\" FROM \"LocalizationProjects\" WHERE \"ID\" IN " +
                        "(SELECT \"ID_LocalizationProject\" FROM \"LocalizationProjectsGlossaries\" WHERE \"ID_Glossary\"=@GlossaryId))";
                    var getGlossaryLocaleParam = new { GlossaryId = glossaryId };
                    this.LogQuery(getGlossaryLocaleSql, getGlossaryLocaleParam);
                    var locale = await dbConnection
                        .QueryFirstAsync<Locale>(
                            sql: getGlossaryLocaleSql,
                            param: getGlossaryLocaleParam);
                    return locale;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetLocaleByIdAsync)} {nameof(NpgsqlException)} ",
                      exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetLocaleByIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<IEnumerable<Locale>> GetTranslationLocalesAsync(int glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var getTranslationLocalesQuery =
                        new Query("Locales")
                        .WhereIn("ID",
                            new Query("GlossariesLocales")
                            .Select("ID_Locale")
                            .Where("ID_Glossary", glossaryId));
                    var getTranslationLocalesCompiledQuery = this._compiler.Compile(getTranslationLocalesQuery);
                    this.LogQuery(getTranslationLocalesCompiledQuery);
                    var translationLocalesForTerm = await dbConnection.QueryAsync<Locale>(
                        sql: getTranslationLocalesCompiledQuery.Sql,
                        param: getTranslationLocalesCompiledQuery.NamedBindings);
                    return translationLocalesForTerm;

                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetTranslationLocalesAsync)} {nameof(NpgsqlException)} ",
                      exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetTranslationLocalesAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Получить все термины из всех глоссариев присоедененных к проекту локализации, по id необходимого проекта локализации
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TermWithGlossary>> GetAllTermsFromAllGlossarisInProjectByIdAsync(int projectId)
        {
            string query = "SELECT " +
                            "DISTINCT ON (TS.\"ID\") TS.\"ID\" AS \"ID\"," +
                            "TS.\"SubstringToTranslate\" AS \"SubstringToTranslate\", " +
                            "TS.\"Description\" AS \"Description\", " +
                            "TS.\"Context\" AS \"Context\", " +
                            "TS.\"TranslationMaxLength\" AS \"TranslationMaxLength\", " +
                            "TS.\"ID_FileOwner\" AS \"ID_FileOwner\", " +
                            "TS.\"Value\" AS \"Value\", " +
                            "TS.\"PositionInText\" AS \"PositionInText\", " +
                            "G.\"ID\" AS \"GlossaryId\", " +
                            "G.\"Name\" AS \"GlossaryName\", " +
                            "G.\"Description\" AS \"GlossaryDescription\" " +
                            "FROM \"LocalizationProjects\" AS LP " +
                            "INNER JOIN \"LocalizationProjectsGlossaries\" AS LPG ON LP.\"ID\" = LPG.\"ID_LocalizationProject\" " +
                            "INNER JOIN \"Glossaries\" AS G ON G.\"ID\" = LPG.\"ID_Glossary\" " +
                            "INNER JOIN \"Files\" AS F ON F.\"ID\" = G.\"ID_File\" " +
                            "INNER JOIN \"TranslationSubstrings\" AS TS ON TS.\"ID_FileOwner\" = F.\"ID\" " +
                            "WHERE LP.\"ID\" = @ProjectId";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var param = new { ProjectId = projectId };
                    this.LogQuery(query, param);
                    IEnumerable<TermWithGlossary> allTerms = await dbConnection.QueryAsync<TermWithGlossary>(query, param);
                    return allTerms;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAllTermsFromAllGlossarisInProjectByIdAsync)} {nameof(NpgsqlException)} ",
                      exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAllTermsFromAllGlossarisInProjectByIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }
        /// <summary>
        /// Удаление всех терминов глоссария
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        public async Task DeleteTermsByGlossaryAsync(int glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var queryGetTranslationSubstringsID = new Query("GlossariesStrings").Where("ID_Glossary", glossaryId).Select("GlossariesStrings.ID_String");
                    var compiledQueryGetTranslationSubstringsID = this._compiler.Compile(queryGetTranslationSubstringsID);
                    this.LogQuery(compiledQueryGetTranslationSubstringsID);
                    var idsTranslationSubstrings = await dbConnection.QueryAsync<int>(
                        sql: compiledQueryGetTranslationSubstringsID.Sql,
                        param: compiledQueryGetTranslationSubstringsID.NamedBindings);

                    var queryTranslationSubstrings = new Query("TranslationSubstrings").WhereIn("ID", idsTranslationSubstrings).AsDelete();
                    var compiledQueryTranslationSubstrings = this._compiler.Compile(queryTranslationSubstrings);
                    this.LogQuery(compiledQueryTranslationSubstrings);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryTranslationSubstrings.Sql,
                        param: compiledQueryTranslationSubstrings.NamedBindings);

                    var queryGlossariesStrings = new Query("GlossariesStrings").Where("ID_Glossary", glossaryId).AsDelete();
                    var compiledQueryGlossariesStrings = this._compiler.Compile(queryGlossariesStrings);
                    this.LogQuery(compiledQueryGlossariesStrings);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesStrings.Sql,
                        param: compiledQueryGlossariesStrings.NamedBindings);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.DeleteTermsByGlossaryAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.DeleteTermsByGlossaryAsync)} {nameof(Exception)} ", exception);
            }
        }
    }
}
