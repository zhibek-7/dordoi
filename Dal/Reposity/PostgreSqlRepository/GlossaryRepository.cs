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

        public Task<Guid?> AddAsync(Glossary item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Glossary>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var selectAllGlossariesSql = "SELECT * FROM glossaries";
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

        public async Task<Glossary> GetByIDAsync(Guid id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var getGlossaryByIdSql = "SELECT * FROM glossaries WHERE id = @GlossaryId LIMIT 1";
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

        public async Task<Glossary> GetByFileIdAsync(Guid fileId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sql = "SELECT * FROM glossaries WHERE id_file = @FileId LIMIT 1";
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

        public Task<bool> RemoveAsync(Guid id)
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
                        "UPDATE glossaries SET name=@Name, description=@Description, id_file=@ID_File " +
                        "WHERE id=@id";
                    var updateGlossaryParam = item;
                    this.LogQuery(updateGlossarySql, item.GetType(), updateGlossaryParam);
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

        public async Task DeleteTermAsync(Guid glossaryId, Guid termId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var deleteGlossaryStingAssotiationSql =
                        "DELETE FROM glossaries_strings " +
                        "WHERE id_glossary = @GlossaryId AND id_string = @TermId";
                    var deleteGlossaryStingAssotiationParam = new { GlossaryId = glossaryId, TermId = termId };
                    this.LogQuery(deleteGlossaryStingAssotiationSql, deleteGlossaryStingAssotiationParam);
                    await dbConnection
                        .ExecuteAsync(
                            sql: deleteGlossaryStingAssotiationSql,
                            param: deleteGlossaryStingAssotiationParam);

                    var deleteStingSql = "DELETE FROM translation_substrings WHERE id = @TermId";
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

        public async Task<Guid?> AddNewTermAsync(Guid glossaryId, TranslationSubstring newTerm, Guid? partOfSpeechId)
        {
            var glossary = await this.GetByIDAsync(id: glossaryId);
            newTerm.id_file_owner = glossary.ID_File;
            newTerm.word_count = TranslationSubstringRepository.GetWordsCount(newTerm.substring_to_translate);
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var insertNewStingSql =
                        "INSERT INTO translation_substrings " +
                        "(" +
                        "substring_to_translate, " +
                        "description, " +
                        "context_file, " +
                        "translation_max_length, " +
                        "id_file_owner, " +
                        "value, " +
                        "position_in_text, " +
                        "word_count" +
                        ") VALUES " +
                        "(" +
                        "@substring_to_translate, " +
                        "@description, " +
                        "@context_file, " +
                        "@translation_max_length, " +
                        "@id_file_owner, " +
                        "@value, " +
                        "@position_in_text, " +
                        "@word_count" +
                        ") " +
                        "RETURNING id";
                    var insertNewStingParam = newTerm;
                    this.LogQuery(insertNewStingSql, newTerm.GetType(), insertNewStingParam);
                    var idOfNewTerm = await dbConnection
                        .ExecuteScalarAsync<Guid>(
                            sql: insertNewStingSql,
                            param: insertNewStingParam);

                    var instertGlossaryStringAssotiationSql =
                        "INSERT INTO glossaries_strings (id_glossary, id_string,id_part_of_speech) VALUES (@glossary_id, @string_id, @parts_of_speechId)";
                    var instertGlossaryStringAssotiationParam = new { glossary_id = glossaryId, string_id = idOfNewTerm, parts_of_speechId = partOfSpeechId };
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
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.AddNewTermAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task UpdateTermAsync(Guid glossaryId, TranslationSubstring updatedTerm, Guid? partOfSpeechId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {


                    var updateTermSql =
                        "UPDATE translation_substrings SET " +
                        "substring_to_translate=@substring_to_translate, " +
                        "description=@description, " +
                        "context=@context, " +
                        "translation_max_length=@translation_max_length, " +
                        "id_file_owner=@id_file_owner, " +
                        "value=@value, " +
                        "position_in_text=@position_in_text " +
                        "WHERE id=@id";
                    var updateTermParam = updatedTerm;
                    this.LogQuery(updateTermSql, updatedTerm.GetType(), updateTermParam);
                    await dbConnection.ExecuteAsync(
                        sql: updateTermSql,
                        param: updateTermParam);

                    var updateTermPartOfSpeechIdSql =
                        "UPDATE glossaries_strings SET " +
                        "id_part_of_speech=@PartOfSpeechId " +
                        "WHERE id_string=@StringId ";// +
                                                     //"AND id_glossary=@GlossaryId";
                    var updateTermPartOfSpeechIdParam = new { GlossaryId = glossaryId, StringId = updatedTerm.id, PartOfSpeechId = partOfSpeechId };
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
            { "id", "translation_substrings.id" },
            { "substring_to_translate", "translation_substrings.substring_to_translate" },
            { "description", "translation_substrings.description" },
            { "context", "translation_substrings.context" },
            { "translation_max_length", "translation_substrings.translation_max_length" },
            { "id_fileowner", "translation_substrings.id_file_owner" },
            { "value", "translation_substrings.value" },
            { "positionin_text", "translation_substrings.position_in_text" },
        };

        public async Task<IEnumerable<Term>> GetAssotiatedTermsByGlossaryIdAsync(
            Guid? glossaryId,
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

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var assotiatedTerms = await dbConnection.QueryAsync<Term>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
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

        public async Task<int?> GetAssotiatedTermsCountAsync(Guid? glossaryId, string termPart)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetAssotiatedTermsQuery(glossaryId, termPart).AsCount();
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var assotiatedTermsCount = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                        );
                    return assotiatedTermsCount;

                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                      $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAssotiatedTermsCountAsync)} {nameof(NpgsqlException)} ",
                      exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.GetAssotiatedTermsCountAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        private Query GetAssotiatedTermsQuery(Guid? glossaryId, string termPart)
        {
            var query =
                new Query("glossaries_strings")
                    .LeftJoin("translation_substrings", "translation_substrings.id", "glossaries_strings.id_string")
                    .Select(
                        "translation_substrings.id",
                        "translation_substrings.substring_to_translate",
                        "translation_substrings.description",
                        "translation_substrings.context",
                        "translation_substrings.translation_max_length",
                        "translation_substrings.id_file_owner",
                        "translation_substrings.value",
                        "translation_substrings.position_in_text",
                        "glossaries_strings.id_part_of_speech as part_of_speech_id")
                    .Select(
                        new Query("translation_substrings_locales")
                            .LeftJoin("translations", join =>
                                join.On("translations.id_string", "translation_substrings_locales.id_translation_substrings")
                                    .On("translations.id_locale", "translation_substrings_locales.id_locale"))
                            .SelectRaw("COUNT(translations.translated) = 0")
                            .Where("translations.translated", "<>", "''")
                            .WhereRaw("translation_substrings_locales.id_translation_substrings=translation_substrings.id"),
                        alias: "is_editable");

            if (glossaryId != null)
            {
                query = query.Where("glossaries_strings.id_glossary", glossaryId);
            }

            if (!string.IsNullOrEmpty(termPart))
            {
                var patternString = $"%{termPart}%";
                query = query.WhereLike("translation_substrings.substring_to_translate", patternString);
            }

            return query;
        }

        public async Task<Locale> GetLocaleByIdAsync(Guid glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects_glossaries as lpg")
                        .Where("id_glossary", glossaryId)
                        .RightJoin("localization_projects as lp", "lpg.id_localization_project", "lp.id")
                        .RightJoin("locales as l", "lp.id_source_locale", "l.id")
                        .Select("l.*");
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var locale = await dbConnection
                        .QueryFirstAsync<Locale>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
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

        public async Task<Locale> GetLocaleByTermByIdAsync(Guid termId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sqlString = @"SELECT l.*
                    FROM public.locales as l
            left join public.glossaries_locales as gl
            on l.id=gl.id_locale
            left join public.glossaries as g
            on gl.id_glossary = g.id
            left join public.glossaries_strings as gs
            on g.id=gs.id_glossary
            where gs.id_glossary =" + termId;

                    this.LogQuery(sqlString);
                    return await dbConnection.QueryFirstAsync<Locale>(sqlString);
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

        public async Task<IEnumerable<Locale>> GetTranslationLocalesAsync(Guid glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var getTranslationLocalesQuery =
                        new Query("locales")
                        .WhereIn("id",
                            new Query("glossaries_locales")
                            .Select("id_locale")
                            .Where("id_glossary", glossaryId));
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
        public async Task<IEnumerable<TermWithGlossary>> GetAllTermsFromAllGlossarisInProjectByIdAsync(Guid projectId)
        {
            string query = "SELECT " +
                            "DISTINCT ON (TS.id) TS.id AS id," +
                            "TS.substring_to_translate AS substring_to_translate, " +
                            "TS.description AS description, " +
                            "TS.context AS context, " +
                            "TS.translation_max_length AS translation_max_length, " +
                            "TS.id_file_owner AS id_file_owner, " +
                            "TS.value AS value, " +
                            "TS.position_in_text AS position_in_text, " +
                            "G.id As glossary_id, " +
                            "G.name_text AS glossary_name, " +
                            "G.description AS glossary_description " +
                            "FROM localization_projects AS LP " +
                            "INNER JOIN localization_projects_glossaries AS LPG ON LP.id = LPG.id_localization_project " +
                            "INNER JOIN glossaries AS G ON G.id = LPG.id_glossary " +
                            "INNER JOIN files AS F ON F.id = G.id_file " +
                            "INNER JOIN translation_substrings AS TS ON TS.id_file_owner = F.id " +
                            "WHERE LP.id = @ProjectId";

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
        public async Task DeleteTermsByGlossaryAsync(Guid glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var queryGetTranslationSubstringsID = new Query("glossaries_strings").Where("id_glossary", glossaryId).Select("glossaries_strings.id_string");
                    var compiledQueryGetTranslationSubstringsID = _compiler.Compile(queryGetTranslationSubstringsID);
                    LogQuery(compiledQueryGetTranslationSubstringsID);
                    var idsTranslationSubstrings = await dbConnection.QueryAsync<Guid>(
                        sql: compiledQueryGetTranslationSubstringsID.Sql,
                        param: compiledQueryGetTranslationSubstringsID.NamedBindings);

                    var queryTranslationSubstrings = new Query("translation_substrings").WhereIn("id", idsTranslationSubstrings).AsDelete();
                    var compiledQueryTranslationSubstrings = _compiler.Compile(queryTranslationSubstrings);
                    LogQuery(compiledQueryTranslationSubstrings);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryTranslationSubstrings.Sql,
                        param: compiledQueryTranslationSubstrings.NamedBindings);

                    var queryGlossariesStrings = new Query("glossaries_strings").Where("id_glossary", glossaryId).AsDelete();
                    var compiledQueryGlossariesStrings = _compiler.Compile(queryGlossariesStrings);
                    LogQuery(compiledQueryGlossariesStrings);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesStrings.Sql,
                        param: compiledQueryGlossariesStrings.NamedBindings);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.DeleteTermsByGlossaryAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossaryRepository)}.{nameof(GlossaryRepository.DeleteTermsByGlossaryAsync)} {nameof(Exception)} ", exception);
            }
        }
    }
}
