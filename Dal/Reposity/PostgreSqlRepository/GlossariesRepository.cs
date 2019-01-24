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


namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossariesRepository : BaseRepository, IGlossariesRepository
    {
        public GlossariesRepository(string connectionStr) : base(connectionStr)
        {
        }

        /// <summary>
        /// Возвращает все строки запроса (без группировки по объектам)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Glossaries>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Glossaries")
                        .LeftJoin("GlossariesLocales", "GlossariesLocales.ID_Glossary", "Glossaries.ID")
                        .LeftJoin("Locales", "Locales.ID", "GlossariesLocales.ID_Locale")
                        .LeftJoin("LocalizationProjectsGlossaries", "LocalizationProjectsGlossaries.ID_Glossary", "Glossaries.ID")
                        .LeftJoin("LocalizationProjects", "LocalizationProjects.ID", "LocalizationProjectsGlossaries.ID_LocalizationProject")
                        .Select(
                            "Glossaries.ID",
                            "Glossaries.Name",
                            "Locales.Name as LocaleName",
                            "LocalizationProjects.Name as LocalizationProjectName"
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
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(GlossariesRepository)}.{nameof(GlossariesRepository.GetAllAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Добавление нового глоссария
        /// </summary>
        /// <param name="glossary">Новый глоссарий</param>
        /// <returns></returns>
        public async Task AddNewGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Добавление нового глоссария
                    var newGlossaries = new
                    {
                        Name = glossary.Name,
                        Description = glossary.Description,
                        ID_File = (int?)null
                    };
                    var query = new Query("Glossaries").AsInsert(newGlossaries, true); //true - вернуть сгенерированный id нового объекта
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    //После выполнение запроса получаем сгенерированный id нового объекта
                    var idOfNewGlossary = await dbConnection
                        .ExecuteScalarAsync<int>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);

                    //Добавление в таблицу "GlossariesLocales" записей связи глоссария с языками перевода (Glossaries с Locales)
                    var GlossariesLocales = glossary.LocalesIds.Select(t => new
                    {
                        ID_Glossary = idOfNewGlossary,
                        ID_Locale = t
                    }).ToList();
                    foreach (var element in GlossariesLocales)
                    {
                        var queryGlossariesLocales = new Query("GlossariesLocales").AsInsert(element);
                        var compiledQueryGlossariesLocales = _compiler.Compile(queryGlossariesLocales);
                        LogQuery(compiledQueryGlossariesLocales);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryGlossariesLocales.Sql,
                                param: compiledQueryGlossariesLocales.NamedBindings);
                    }

                    //Добавление в таблицу "LocalizationProjectsGlossaries" записей связи глоссария с проектами локализации (Glossaries с LocalizationProjects)
                    var LocalizationProjectsGlossaries = glossary.LocalizationProjectsIds.Select(t => new
                    {
                        ID_Glossary = idOfNewGlossary,
                        ID_LocalizationProject = t
                    }).ToList();
                    foreach (var element in LocalizationProjectsGlossaries)
                    {
                        var queryLocalizationProjectsGlossaries = new Query("LocalizationProjectsGlossaries").AsInsert(element);
                        var compiledQueryLocalizationProjectsGlossaries = _compiler.Compile(queryLocalizationProjectsGlossaries);
                        LogQuery(compiledQueryLocalizationProjectsGlossaries);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryLocalizationProjectsGlossaries.Sql,
                                param: compiledQueryLocalizationProjectsGlossaries.NamedBindings);
                    }
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
        /// Возвращает глоссарий для редактирования (без группировки по объектам)
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        public async Task<IEnumerable<Glossaries>> GetGlossaryForEditAsync(int glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Glossaries").Where("Glossaries.ID", glossaryId)
                        .LeftJoin("GlossariesLocales", "GlossariesLocales.ID_Glossary", "Glossaries.ID")
                        .LeftJoin("Locales", "Locales.ID", "GlossariesLocales.ID_Locale")
                        .LeftJoin("LocalizationProjectsGlossaries", "LocalizationProjectsGlossaries.ID_Glossary", "Glossaries.ID")
                        .LeftJoin("LocalizationProjects", "LocalizationProjects.ID", "LocalizationProjectsGlossaries.ID_LocalizationProject")
                        .Select(
                            "Glossaries.ID",
                            "Glossaries.Name",
                            "Glossaries.Description",
                            "Glossaries.ID_File",
                            "Locales.ID as LocaleID",
                            "LocalizationProjects.ID as LocalizationProjectID"
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
        /// Сохранение изменений в глоссарии
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий</param>
        /// <returns></returns>
        public async Task EditGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Обновление глоссария
                    var editedGlossaries = new
                    {
                        Name = glossary.Name,
                        Description = glossary.Description,
                        ID_File = glossary.ID_File
                    };
                    var query = new Query("Glossaries").Where("ID", glossary.ID).AsUpdate(editedGlossaries);
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
                    

                    //Пересоздание связей глоссария с языками перевода (Glossaries с Locales)
                    if(glossary.LocalesIds != null && glossary.LocalesIds.Count() > 0)
                        await EditGlossariesLocalesAsync(glossary.ID, glossary.LocalesIds);
                    
                    //Пересоздание связей глоссария с проектами локализации (Glossaries с LocalizationProjects)
                    if(glossary.LocalizationProjectsIds != null && glossary.LocalizationProjectsIds.Count() > 0)
                        await EditGlossariesLocalizationProjectsAsync(glossary.ID, glossary.LocalizationProjectsIds);
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
        /// Пересоздание связей глоссария с языками перевода (Glossaries с Locales)
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <param name="localesIds">Выбранные языки перевода</param>
        /// <returns></returns>
        public async Task EditGlossariesLocalesAsync(int glossaryId, IEnumerable<int?> localesIds)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var queryGlossariesLocalesDelete = new Query("GlossariesLocales").Where("ID_Glossary", glossaryId).AsDelete();
                    var compiledQueryGlossariesLocalesDelete = _compiler.Compile(queryGlossariesLocalesDelete);
                    LogQuery(compiledQueryGlossariesLocalesDelete);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesLocalesDelete.Sql,
                        param: compiledQueryGlossariesLocalesDelete.NamedBindings);
                    var GlossariesLocales = localesIds.Select(t => new
                    {
                        ID_Glossary = glossaryId,
                        ID_Locale = t
                    }).ToList();
                    foreach (var element in GlossariesLocales)
                    {
                        var queryGlossariesLocalesInsert = new Query("GlossariesLocales").AsInsert(element);
                        var compiledQueryGlossariesLocalesInsert = _compiler.Compile(queryGlossariesLocalesInsert);
                        LogQuery(compiledQueryGlossariesLocalesInsert);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryGlossariesLocalesInsert.Sql,
                                param: compiledQueryGlossariesLocalesInsert.NamedBindings);
                    }

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
        /// Пересоздание связей глоссария с проектами локализации (Glossaries с LocalizationProjects)
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <param name="localizationProjectsIds">Выбранные проекты локализации</param>
        /// <returns></returns>
        public async Task EditGlossariesLocalizationProjectsAsync(int glossaryId, IEnumerable<int?> localizationProjectsIds)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var queryLocalizationProjectsGlossariesDelete = new Query("LocalizationProjectsGlossaries").Where("ID_Glossary", glossaryId).AsDelete();
                    var compiledQueryLocalizationProjectsGlossariesDelete = _compiler.Compile(queryLocalizationProjectsGlossariesDelete);
                    LogQuery(compiledQueryLocalizationProjectsGlossariesDelete);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryLocalizationProjectsGlossariesDelete.Sql,
                        param: compiledQueryLocalizationProjectsGlossariesDelete.NamedBindings);
                    var LocalizationProjectsGlossaries = localizationProjectsIds.Select(t => new
                    {
                        ID_Glossary = glossaryId,
                        ID_LocalizationProject = t
                    }).ToList();
                    foreach (var element in LocalizationProjectsGlossaries)
                    {
                        var queryLocalizationProjectsGlossariesInsert = new Query("LocalizationProjectsGlossaries").AsInsert(element);
                        var compiledQueryLocalizationProjectsGlossariesInsert = _compiler.Compile(queryLocalizationProjectsGlossariesInsert);
                        LogQuery(compiledQueryLocalizationProjectsGlossariesInsert);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryLocalizationProjectsGlossariesInsert.Sql,
                                param: compiledQueryLocalizationProjectsGlossariesInsert.NamedBindings);
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
        /// Удаление глоссария
        /// </summary>
        /// <param name="id">Идентификатор глоссария</param>
        /// <returns></returns>
        public async Task DeleteGlossaryAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //при удалени глосария удаляются файл, строки, термины и переводы,
                    //все из TranslationSubstrings и Files удаляется по Glossaries.ID_File,                   
                    //еще Translations (ID_String), на них CommentsImages(ID_Comment),Comments(ID_TranslationSubstrings)                    

                    //Удаление зависимостей
                    #region Get Glossaries.ID_File

                    var queryGetGlossariesID_File = new Query("Glossaries").Where("ID", id).Select("Glossaries.ID_File");
                    var compiledQueryGetGlossariesID_File = this._compiler.Compile(queryGetGlossariesID_File);
                    this.LogQuery(compiledQueryGetGlossariesID_File);
                    var idsFile = await dbConnection.QueryAsync<int?>( //QueryFirstOrDefaultAsync
                        sql: compiledQueryGetGlossariesID_File.Sql,
                        param: compiledQueryGetGlossariesID_File.NamedBindings
                    );

                    var idFile = idsFile.FirstOrDefault();

                    #endregion

                    if (idFile != null)
                    {
                        #region Таблицы с прямой ссылкой на "Files" (Glossaries.ID_File)

                        //var queryUserActions = new Query("UserActions").Where("ID_File", idFile).AsDelete();
                        //var compiledQueryUserActions = this._compiler.Compile(queryUserActions);
                        //this.LogQuery(compiledQueryUserActions);
                        //await dbConnection.ExecuteAsync(
                        //    sql: compiledQueryUserActions.Sql,
                        //    param: compiledQueryUserActions.NamedBindings);
                        var queryUserActions = new Query("UserActions").Where("ID_File", idFile)
                            .AsUpdate(new[] { "ID_File" },
                            new object[] { null });
                        var compiledQueryUserActions = this._compiler.Compile(queryUserActions);
                        this.LogQuery(compiledQueryUserActions);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryUserActions.Sql,
                            param: compiledQueryUserActions.NamedBindings);


                        var queryTasks = new Query("Tasks").Where("ID_File", idFile).AsDelete();
                        var compiledQueryTasks = this._compiler.Compile(queryTasks);
                        this.LogQuery(compiledQueryTasks);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryTasks.Sql,
                            param: compiledQueryTasks.NamedBindings);

                        var queryFilesLocales = new Query("FilesLocales").Where("ID_File", idFile).AsDelete();
                        var compiledQueryFilesLocales = this._compiler.Compile(queryFilesLocales);
                        this.LogQuery(compiledQueryFilesLocales);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryFilesLocales.Sql,
                            param: compiledQueryFilesLocales.NamedBindings);

                        #region Таблицы с прямой ссылкой на "TranslationSubstrings" (Glossaries.ID_File -> TranslationSubstrings.ID_FileOwner => TranslationSubstrings.ID)

                        #region Get TranslationSubstrings.ID

                        var queryGetTranslationSubstringsID1 = new Query("TranslationSubstrings").Where("ID_FileOwner", idFile).Select("TranslationSubstrings.ID");
                        var compiledQueryGetTranslationSubstringsID1 = this._compiler.Compile(queryGetTranslationSubstringsID1);
                        this.LogQuery(compiledQueryGetTranslationSubstringsID1);
                        var idsTranslationSubstrings1 = await dbConnection.QueryAsync<int>(
                            sql: compiledQueryGetTranslationSubstringsID1.Sql,
                            param: compiledQueryGetTranslationSubstringsID1.NamedBindings);

                        var queryGetTranslationSubstringsID2 = new Query("GlossariesStrings").Where("ID_Glossary", id).Select("GlossariesStrings.ID_String");
                        var compiledQueryGetTranslationSubstringsID2 = this._compiler.Compile(queryGetTranslationSubstringsID2);
                        this.LogQuery(compiledQueryGetTranslationSubstringsID2);
                        var idsTranslationSubstrings2 = await dbConnection.QueryAsync<int>(
                            sql: compiledQueryGetTranslationSubstringsID2.Sql,
                            param: compiledQueryGetTranslationSubstringsID2.NamedBindings);

                        var idsTranslationSubstrings = new List<int>();
                        idsTranslationSubstrings.AddRange(idsTranslationSubstrings1);
                        idsTranslationSubstrings.AddRange(idsTranslationSubstrings2);

                        #endregion

                        if (idsTranslationSubstrings != null && idsTranslationSubstrings.Count() > 0)
                        {

                            var queryUserActions2 = new Query("UserActions")
                                .WhereIn("ID_String", idsTranslationSubstrings)
                                .AsUpdate(new[] { "ID_String" },
                                    new object[] { null });
                            var compiledQueryUserActions2 = this._compiler.Compile(queryUserActions2);
                            this.LogQuery(compiledQueryUserActions2);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryUserActions2.Sql,
                                param: compiledQueryUserActions2.NamedBindings);

                            var queryStringsContextImages = new Query("StringsContextImages")
                                .WhereIn("ID_String", idsTranslationSubstrings).AsDelete();
                            var compiledQueryStringsContextImages = this._compiler.Compile(queryStringsContextImages);
                            this.LogQuery(compiledQueryStringsContextImages);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryStringsContextImages.Sql,
                                param: compiledQueryStringsContextImages.NamedBindings);

                            var queryTranslationsubStringsLocales = new Query("TranslationsubStringsLocales")
                                .WhereIn("Id_TranslationSubStrings", idsTranslationSubstrings).AsDelete();
                            var compiledQueryTranslationsubStringsLocales =
                                this._compiler.Compile(queryTranslationsubStringsLocales);
                            this.LogQuery(compiledQueryTranslationsubStringsLocales);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryTranslationsubStringsLocales.Sql,
                                param: compiledQueryTranslationsubStringsLocales.NamedBindings);

                            #region Translations

                            var queryGetTranslationsID = new Query("Translations")
                                .WhereIn("ID_String", idsTranslationSubstrings).Select("Translations.ID");
                            var compiledQueryGetTranslationsID = this._compiler.Compile(queryGetTranslationsID);
                            this.LogQuery(compiledQueryGetTranslationsID);
                            var idsTranslations = await dbConnection.QueryAsync<int>(
                                sql: compiledQueryGetTranslationsID.Sql,
                                param: compiledQueryGetTranslationsID.NamedBindings);

                            var queryUserActions3 = new Query("UserActions").WhereIn("ID_Translation", idsTranslations)
                                .AsUpdate(new[] { "ID_Translation" },
                                    new object[] { null });
                            var compiledQueryUserActions3 = this._compiler.Compile(queryUserActions3);
                            this.LogQuery(compiledQueryUserActions3);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryUserActions3.Sql,
                                param: compiledQueryUserActions3.NamedBindings);

                            #endregion

                            var queryTranslations = new Query("Translations")
                                .WhereIn("ID_String", idsTranslationSubstrings).AsDelete();
                            var compiledQueryTranslations = this._compiler.Compile(queryTranslations);
                            this.LogQuery(compiledQueryTranslations);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryTranslations.Sql,
                                param: compiledQueryTranslations.NamedBindings);

                            #region Таблицы с прямой ссылкой на "Comments" (Glossaries.ID_File -> TranslationSubstrings.ID_FileOwner -> TranslationSubstrings.ID -> Comments.Id_TranslationSubStrings => Comments.ID_Comment)

                            #region Get Comments.ID

                            var queryGetCommentsID = new Query("Comments")
                                .WhereIn("ID_TranslationSubstrings", idsTranslationSubstrings).Select("Comments.ID");
                            var compiledQueryGetCommentsID = this._compiler.Compile(queryGetCommentsID);
                            this.LogQuery(compiledQueryGetCommentsID);
                            var idsComments = await dbConnection.QueryAsync<int>(
                                sql: compiledQueryGetCommentsID.Sql,
                                param: compiledQueryGetCommentsID.NamedBindings);

                            #endregion

                            var queryCommentsImages = new Query("CommentsImages").WhereIn("ID_Comment", idsComments)
                                .AsDelete();
                            var compiledQueryCommentsImages = this._compiler.Compile(queryCommentsImages);
                            this.LogQuery(compiledQueryCommentsImages);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryCommentsImages.Sql,
                                param: compiledQueryCommentsImages.NamedBindings);

                            #region "Comments" (Glossaries.ID_File -> TranslationSubstrings)

                            var queryComments = new Query("Comments")
                                .WhereIn("ID_TranslationSubstrings", idsTranslationSubstrings).AsDelete();
                            var compiledQueryComments = this._compiler.Compile(queryComments);
                            this.LogQuery(compiledQueryComments);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryComments.Sql,
                                param: compiledQueryComments.NamedBindings);

                            #endregion

                            #endregion

                            #region "TranslationSubstrings" (Glossaries.ID_File)

                            var queryTranslationSubstrings = new Query("TranslationSubstrings")
                                .WhereIn("ID", idsTranslationSubstrings).AsDelete();
                            var compiledQueryTranslationSubstrings = this._compiler.Compile(queryTranslationSubstrings);
                            this.LogQuery(compiledQueryTranslationSubstrings);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryTranslationSubstrings.Sql,
                                param: compiledQueryTranslationSubstrings.NamedBindings);

                            //var queryTranslationSubstringsID_FileOwner = new Query("TranslationSubstrings").Where("ID_FileOwner", idFile).AsDelete();
                            //var compiledQueryTranslationSubstringsID_FileOwner = this._compiler.Compile(queryTranslationSubstringsID_FileOwner);
                            //this.LogQuery(compiledQueryTranslationSubstringsID_FileOwner);
                            //await dbConnection.ExecuteAsync(
                            //    sql: compiledQueryTranslationSubstringsID_FileOwner.Sql,
                            //    param: compiledQueryTranslationSubstringsID_FileOwner.NamedBindings);

                            #endregion

                        }

                        #endregion

                        #endregion
                    }


                    #region Таблицы с прямой ссылкой на "Glossaries"

                    var queryGlossariesStrings = new Query("GlossariesStrings").Where("ID_Glossary", id).AsDelete();
                    var compiledQueryGlossariesStrings = this._compiler.Compile(queryGlossariesStrings);
                    this.LogQuery(compiledQueryGlossariesStrings);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesStrings.Sql,
                        param: compiledQueryGlossariesStrings.NamedBindings);

                    var queryGlossariesLocales = new Query("GlossariesLocales").Where("ID_Glossary", id).AsDelete();
                    var compiledQueryGlossariesLocales = this._compiler.Compile(queryGlossariesLocales);
                    this.LogQuery(compiledQueryGlossariesLocales);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesLocales.Sql,
                        param: compiledQueryGlossariesLocales.NamedBindings);

                    var queryLocalizationProjectsGlossaries = new Query("LocalizationProjectsGlossaries").Where("ID_Glossary", id).AsDelete();
                    var compiledQueryLocalizationProjectsGlossaries = this._compiler.Compile(queryLocalizationProjectsGlossaries);
                    this.LogQuery(compiledQueryLocalizationProjectsGlossaries);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryLocalizationProjectsGlossaries.Sql,
                        param: compiledQueryLocalizationProjectsGlossaries.NamedBindings);

                    #endregion

                    //Удаление глоссария
                    #region "Glossaries"

                    var query = new Query("Glossaries").Where("ID", id).AsDelete();
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    #endregion

                    //Удаление зависимостей
                    #region "Files" (Glossaries.ID_File)

                    var queryFiles = new Query("Files").Where("ID", idFile).AsDelete();
                    var compiledQueryFiles = this._compiler.Compile(queryFiles);
                    this.LogQuery(compiledQueryFiles);
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

    }
}
