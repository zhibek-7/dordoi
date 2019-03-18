using DAL.Context;
using Dapper;
using Models.Interfaces.Repository;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Npgsql;
using System;
using Utilities.Data;
using Utilities.Mail;


namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossariesRepository : BaseRepository, IGlossariesRepository
    {
        private FilesRepository fr;
        public GlossariesRepository(string connectionStr) : base(connectionStr)
        {
            fr = new FilesRepository(connectionStr);
        }



        public static Dictionary<string, string> SortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "glossaries.id" },
            { "name_text", "glossaries.name_text" },
            { "locales_name", "locales_g.locales_name"},//"locales.name_text" },
            { "localization_projects_name", "localization_projects_g.localization_projects_name"},//"localization_projects.name_text" },
            { "string_count", "string_count" }
        };


        /// <summary>
        /// Возвращает список глоссариев, со строками перечислений имен связанных объектов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllByUserIdAsync(
            int? userId,
            int offset,
            int limit,
            int? projectId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            if (sortBy == null || !sortBy.Any())
            {
                sortBy = new[] { "id" };
            }

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = GetAllByUserIdQuery(
                        userId,
                        projectId,
                        searchString);

                    query = ApplyPagination(
                        query: query,
                        offset: offset,
                        limit: limit);

                    query = ApplySorting(
                        query: query,
                        columnNamesMappings: GlossariesRepository.SortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var compiledQuery = this._compiler.Compile(query);
                    LogQuery(compiledQuery);

                    var glossaries = await dbConnection.QueryAsync<GlossariesTableViewDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );

                    return glossaries;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllByUserIdAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllByUserIdAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает количество глоссариев.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <returns></returns>
        public async Task<int> GetAllByUserIdCountAsync(
            int? userId,
            int? projectId = null,
            string searchString = null)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = GetAllByUserIdQuery(
                        userId,
                        projectId,
                        searchString);
                    query = query.Distinct().AsCount("glossaries.id");


                    var compiledQuery = _compiler.Compile(query);
                    this.LogQuery(compiledQuery);

                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );
                    return count;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllByUserIdCountAsync)} {nameof(NpgsqlException)} ", exception);
                return 0;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllByUserIdCountAsync)} {nameof(Exception)} ", exception);
                return 0;
            }

        }

        /// <summary>
        /// Возвращает запрос глоссариев (со связанными объектами).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <returns></returns>
        private Query GetAllByUserIdQuery(
            int? userId,
            int? projectId = null,
            string searchString = null)
        {
            try
            {
                var queryLocalesGlossaries = new Query("glossaries")
                    .LeftJoin("glossaries_locales", "glossaries_locales.id_glossary", "glossaries.id")
                    .LeftJoin("locales", "locales.id", "glossaries_locales.id_locale")
                    .Select("glossaries.id as tm_id")
                    .GroupBy("glossaries.id")
                    .SelectRaw("string_agg(locales.name_text, ', ' order by locales.name_text) as locales_name");

                var queryLocalizationProjectsGlossaries = new Query("glossaries")
                    .LeftJoin("localization_projects_glossaries", "localization_projects_glossaries.id_glossary", "glossaries.id")
                    .LeftJoin("localization_projects", "localization_projects.id", "localization_projects_glossaries.id_localization_project")
                    .Join("participants", "participants.id_localization_project", "localization_projects.id")
                    .WhereTrue("participants.active")
                    .Where("participants.id_user", (int)userId)
                    .Select("glossaries.id as tm_id")
                    .GroupBy("glossaries.id")
                    .SelectRaw("string_agg(localization_projects.name_text, ', ' order by localization_projects.name_text) as localization_projects_name");


                var query = new Query("glossaries")
                    .With("locales_g", queryLocalesGlossaries)
                    .With("localization_projects_g", queryLocalizationProjectsGlossaries)
                    .Join("locales_g", "locales_g.tm_id", "glossaries.id")
                    .Join("localization_projects_g", "localization_projects_g.tm_id", "glossaries.id")
                    .Select(
                        "glossaries.id",
                        "glossaries.name_text",
                        "locales_g.locales_name",
                        "localization_projects_g.localization_projects_name"
                    );

                if (projectId != null)
                {
                    query = query
                        .Join("localization_projects_glossaries", "localization_projects_glossaries.id_glossary", "glossaries.id")
                        .Where("localization_projects_glossaries.id_localization_project", projectId);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchPattern = $"%{searchString}%";
                    query = query.WhereLike("glossaries.name_text", searchPattern);
                }

                var compiledQuery = _compiler.Compile(query);
                LogQuery(compiledQuery);

                return query;
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllByUserIdQuery)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllByUserIdQuery)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<IEnumerable<Glossaries>> GetAllAsync(int? userId, int? projectId)
        {
            return null;
        }

        /// <summary>
        /// Возвращает глоссарий для редактирования (без группировки по объектам).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Glossaries>> GetGlossaryForEditAsync(int glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("glossaries").Where("glossaries.id", glossaryId)
                        .LeftJoin("glossaries_locales", "glossaries_locales.id_glossary", "glossaries.id")
                        .LeftJoin("locales", "locales.id", "glossaries_locales.id_locale")
                        .LeftJoin("localization_projects_glossaries", "localization_projects_glossaries.id_glossary", "glossaries.id")
                        .LeftJoin("localization_projects", "localization_projects.id", "localization_projects_glossaries.id_localization_project")
                        .Select(
                            "glossaries.id",
                            "glossaries.name_text",
                            "glossaries.description",
                            "glossaries.id_file",
                            "locales.id as locale_id",
                            "localization_projects.id as Localization_Project_ID"
                        );
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var glossaries = await dbConnection.QueryAsync<Glossaries>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return glossaries;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetGlossaryForEditAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetGlossaryForEditAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Добавление нового глоссария.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="glossary">Новый глоссарий.</param>
        /// <returns></returns>
        public async Task AddNewGlossaryAsync(int userId, GlossariesForEditingDTO glossary)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Добавление нового глоссария
                    var newGlossaries = new
                    {
                        name_text = glossary.Name_text,
                        description = glossary.Description,
                        id_file = glossary.ID_File
                    };
                    var query = new Query("glossaries").AsInsert(newGlossaries, true); //true - вернуть сгенерированный id нового объекта
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    //После выполнение запроса получаем сгенерированный id нового объекта
                    var idOfNewGlossary = await dbConnection
                        .ExecuteScalarAsync<int>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);

                    //Добавление в таблицу "GlossariesLocales" записей связи глоссария с языками перевода (Glossaries с Locales)
                    var loc = ConvertData.ConverLocale(glossary.Locales_Ids);

                    await EditGlossariesLocalesAsync(idOfNewGlossary, loc, glossary.ID_File, false);

                    //Добавление в таблицу "localization_projectsGlossaries" записей связи глоссария с проектами локализации (Glossaries с localization_projects)
                    await EditGlossariesLocalizationProjectsAsync(userId, idOfNewGlossary, glossary.Localization_Projects_Ids, false);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.AddNewGlossaryAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.AddNewGlossaryAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="glossary">Отредактированный глоссарий.</param>
        /// <returns></returns>
        public async Task EditGlossaryAsync(int userId, GlossariesForEditingDTO glossary)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Обновление глоссария
                    var editedGlossaries = new
                    {
                        name_text = glossary.Name_text,
                        description = glossary.Description,
                        id_file = glossary.ID_File
                    };
                    var query = new Query("glossaries").Where("id", glossary.id).AsUpdate(editedGlossaries);
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);

                    var loc = ConvertData.ConverLocale(glossary.Locales_Ids);
                    //Пересоздание связей глоссария с языками перевода (Glossaries с Locales)
                    await EditGlossariesLocalesAsync(glossary.id, loc, glossary.ID_File);

                    //Пересоздание связей глоссария с проектами локализации (Glossaries с localization_projects)
                    await EditGlossariesLocalizationProjectsAsync(userId, glossary.id, glossary.Localization_Projects_Ids);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.EditGlossaryAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.EditGlossaryAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Пересоздание связей глоссария с языками перевода (Glossaries с Locales).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <param name="localesIds">Выбранные языки перевода.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        private async Task EditGlossariesLocalesAsync(int glossaryId, IEnumerable<int> localesIds, int? filId, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryGlossariesLocalesDelete = new Query("glossaries_locales").Where("id_glossary", glossaryId).AsDelete();
                        var compiledQueryGlossariesLocalesDelete = _compiler.Compile(queryGlossariesLocalesDelete);
                        LogQuery(compiledQueryGlossariesLocalesDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryGlossariesLocalesDelete.Sql,
                            param: compiledQueryGlossariesLocalesDelete.NamedBindings);
                    }

                    var glossariesLocales = localesIds.Select(t => new
                    {
                        id_glossary = glossaryId,
                        id_locale = t
                    }).ToList();

                    foreach (var element in glossariesLocales)
                    {
                        var queryGlossariesLocalesInsert = new Query("glossaries_locales").AsInsert(element);
                        var compiledQueryGlossariesLocalesInsert = _compiler.Compile(queryGlossariesLocalesInsert);
                        LogQuery(compiledQueryGlossariesLocalesInsert);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryGlossariesLocalesInsert.Sql,
                                param: compiledQueryGlossariesLocalesInsert.NamedBindings);
                    }

                    ///обновляем у  файла локали
                    int? fileId = GetIdFile(glossaryId, dbConnection).Result;
                    await this.fr.DeleteTranslationLocalesAsync(fileId: (int)fileId);
                    await this.fr.AddTranslationLocalesAsync(fileId: (int)fileId, localesIds: localesIds);


                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.EditGlossariesLocalesAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.EditGlossariesLocalesAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Пересоздание связей глоссария с проектами локализации (Glossaries с localization_projects).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <param name="localization_projectsIds">Выбранные проекты локализации.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        private async Task EditGlossariesLocalizationProjectsAsync(int userId, int glossaryId, IEnumerable<int?> localization_projectsIds, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryDelete = new Query("localization_projects_glossaries")
                            .Where("id_glossary", glossaryId)
                            .WhereIn("id_localization_project",
                                new Query("localization_projects_glossaries")
                                    .Join("participants", "participants.id_localization_project", "localization_projects_glossaries.id_localization_project")
                                    .WhereTrue("participants.active")
                                    .Where("participants.id_user", userId)
                                    .Where("localization_projects_glossaries.id_glossary", glossaryId)
                                    .Select("localization_projects_glossaries.id_localization_project")
                            )
                            .AsDelete();
                        var compiledQueryDelete = _compiler.Compile(queryDelete);
                        LogQuery(compiledQueryDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryDelete.Sql,
                            param: compiledQueryDelete.NamedBindings);
                    }

                    var localizationProjectsGlossaries = localization_projectsIds.Select(t => new
                    {
                        id_glossary = glossaryId,
                        id_localization_project = t
                    }).ToList();

                    foreach (var element in localizationProjectsGlossaries)
                    {
                        var queryInsert = new Query("localization_projects_glossaries").AsInsert(element);
                        var compiledQueryInsert = _compiler.Compile(queryInsert);
                        LogQuery(compiledQueryInsert);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryInsert.Sql,
                                param: compiledQueryInsert.NamedBindings);
                    }
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.EditGlossariesLocalizationProjectsAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.EditGlossariesLocalizationProjectsAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Удаление глоссария.
        /// </summary>
        /// <param name="id">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task DeleteGlossaryAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //правилом каскадного удаления
                    //при удалени глосария удаляются файл, строки, термины и переводы.                
                    //еще Translations (ID_String), на них CommentsImages(ID_Comment),Comments(ID_TranslationSubstrings).
                    //Таблицы с прямой ссылкой на "Comments" (Glossaries.ID_File -> TranslationSubstrings.ID_FileOwner -> TranslationSubstrings.ID -> Comments.Id_TranslationSubStrings => Comments.ID_Comment) - "CommentsImages".
                    //Таблицы с прямой ссылкой на "Glossaries" - "GlossariesStrings", "GlossariesLocales", "localization_projectsGlossaries".

                    //все из TranslationSubstrings и Files удаляется по Glossaries.ID_File

                    var idFile = await GetIdFile(id, dbConnection);

                    if (idFile != null)
                    {
                        #region Таблицы с прямой ссылкой на "Files" (Glossaries.ID_File) - "Tasks", "FilesLocales".

                        var queryUserActions = new Query("user_actions").Where("id_file", idFile)
                            .AsUpdate(
                                new[] { "id_file" },
                                new object[] { null }
                                );
                        var compiledQueryUserActions = _compiler.Compile(queryUserActions);
                        LogQuery(compiledQueryUserActions);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryUserActions.Sql,
                            param: compiledQueryUserActions.NamedBindings);

                        #region Таблицы с прямой ссылкой на "TranslationSubstrings" (Glossaries.ID_File -> TranslationSubstrings.ID_FileOwner => TranslationSubstrings.ID) - "StringsContextImages", "TranslationsubStringsLocales", "Translations", "Comments".

                        var idsTranslationSubstrings = await GetTranslationSubstrings(id, idFile, dbConnection);


                        if (idsTranslationSubstrings != null && idsTranslationSubstrings.Count() > 0)
                        {
                            var queryUserActions2 = new Query("user_actions").WhereIn("id_string", idsTranslationSubstrings)
                                .AsUpdate(
                                    new[] { "id_string" },
                                    new object[] { null }
                                    );
                            var compiledQueryUserActions2 = _compiler.Compile(queryUserActions2);
                            LogQuery(compiledQueryUserActions2);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryUserActions2.Sql,
                                param: compiledQueryUserActions2.NamedBindings);

                            #region Translations

                            var queryGetTranslationsID = new Query("translations").WhereIn("id_string", idsTranslationSubstrings).Select("translations.id");
                            var compiledQueryGetTranslationsID = _compiler.Compile(queryGetTranslationsID);
                            LogQuery(compiledQueryGetTranslationsID);
                            var idsTranslations = await dbConnection.QueryAsync<int>(
                                sql: compiledQueryGetTranslationsID.Sql,
                                param: compiledQueryGetTranslationsID.NamedBindings);

                            var queryUserActions3 = new Query("user_actions").WhereIn("id_translation", idsTranslations)
                                .AsUpdate(
                                    new[] { "id_translation" },
                                    new object[] { null }
                                    );
                            var compiledQueryUserActions3 = _compiler.Compile(queryUserActions3);
                            LogQuery(compiledQueryUserActions3);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryUserActions3.Sql,
                                param: compiledQueryUserActions3.NamedBindings);

                            #endregion

                            #region "TranslationSubstrings" (Glossaries.ID_File)

                            var queryTranslationSubstrings = new Query("translation_substrings").WhereIn("id", idsTranslationSubstrings).AsDelete();
                            var compiledQueryTranslationSubstrings = _compiler.Compile(queryTranslationSubstrings);
                            LogQuery(compiledQueryTranslationSubstrings);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryTranslationSubstrings.Sql,
                                param: compiledQueryTranslationSubstrings.NamedBindings);

                            #endregion

                        }

                        #endregion

                        #endregion
                    }

                    //Удаление глоссария
                    var query = new Query("glossaries").Where("id", id).AsDelete();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);


                    //Удаление зависимостей
                    #region "Files" (Glossaries.ID_File)

                    var queryFiles = new Query("files").Where("id", idFile).AsDelete();
                    var compiledQueryFiles = _compiler.Compile(queryFiles);
                    LogQuery(compiledQueryFiles);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryFiles.Sql,
                        param: compiledQueryFiles.NamedBindings);

                    #endregion

                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.DeleteGlossaryAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.DeleteGlossaryAsync)} {nameof(Exception)} ", exception);
            }
        }

        private async Task<List<int>> GetTranslationSubstrings(int id, int? idFile, NpgsqlConnection dbConnection)
        {
            var queryGetTranslationSubstringsID1 = new Query("translation_substrings").Where("id_file_owner", idFile)
                .Select("translation_substrings.id");
            var compiledQueryGetTranslationSubstringsID1 = _compiler.Compile(queryGetTranslationSubstringsID1);
            LogQuery(compiledQueryGetTranslationSubstringsID1);
            var idsTranslationSubstrings1 = await dbConnection.QueryAsync<int>(
                sql: compiledQueryGetTranslationSubstringsID1.Sql,
                param: compiledQueryGetTranslationSubstringsID1.NamedBindings);

            var queryGetTranslationSubstringsID2 = new Query("glossaries_strings").Where("id_glossary", id)
                .Select("glossaries_strings.id_string");
            var compiledQueryGetTranslationSubstringsID2 = _compiler.Compile(queryGetTranslationSubstringsID2);
            LogQuery(compiledQueryGetTranslationSubstringsID2);
            var idsTranslationSubstrings2 = await dbConnection.QueryAsync<int>(
                sql: compiledQueryGetTranslationSubstringsID2.Sql,
                param: compiledQueryGetTranslationSubstringsID2.NamedBindings);

            var idsTranslationSubstrings = new List<int>();
            idsTranslationSubstrings.AddRange(idsTranslationSubstrings1);
            idsTranslationSubstrings.AddRange(idsTranslationSubstrings2);
            return idsTranslationSubstrings;
        }

        /// <summary>
        /// Получение ID file зависимостей
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        private async Task<int?> GetIdFile(int id, NpgsqlConnection dbConnection)
        {
            //Удаление зависимостей


            var queryGetGlossariesID_File = new Query("glossaries").Where("id", id).Select("glossaries.id_file");
            var compiledQueryGetGlossariesID_File = _compiler.Compile(queryGetGlossariesID_File);
            LogQuery(compiledQueryGetGlossariesID_File);
            var idFile = await dbConnection.QueryFirstOrDefaultAsync<int?>(
                sql: compiledQueryGetGlossariesID_File.Sql,
                param: compiledQueryGetGlossariesID_File.NamedBindings);
            return idFile;
        }
    }
}
