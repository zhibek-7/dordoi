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
        /// Возвращает все строки запроса (без группировки по объектам).
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
        /// Добавление нового глоссария.
        /// </summary>
        /// <param name="glossary">Новый глоссарий.</param>
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
                        ID_File = glossary.ID_File
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
                    await EditGlossariesLocalesAsync(idOfNewGlossary, glossary.LocalesIds, false);

                    //Добавление в таблицу "LocalizationProjectsGlossaries" записей связи глоссария с проектами локализации (Glossaries с LocalizationProjects)
                    await EditGlossariesLocalizationProjectsAsync(idOfNewGlossary, glossary.LocalizationProjectsIds, false);
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
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий.</param>
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
                    await EditGlossariesLocalesAsync(glossary.ID, glossary.LocalesIds);
                    
                    //Пересоздание связей глоссария с проектами локализации (Glossaries с LocalizationProjects)
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
        /// Пересоздание связей глоссария с языками перевода (Glossaries с Locales).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <param name="localesIds">Выбранные языки перевода.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task EditGlossariesLocalesAsync(int glossaryId, IEnumerable<int?> localesIds, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryGlossariesLocalesDelete = new Query("GlossariesLocales").Where("ID_Glossary", glossaryId).AsDelete();
                        var compiledQueryGlossariesLocalesDelete = _compiler.Compile(queryGlossariesLocalesDelete);
                        LogQuery(compiledQueryGlossariesLocalesDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryGlossariesLocalesDelete.Sql,
                            param: compiledQueryGlossariesLocalesDelete.NamedBindings);
                    }

                    var glossariesLocales = localesIds.Select(t => new
                    {
                        ID_Glossary = glossaryId,
                        ID_Locale = t
                    }).ToList();

                    foreach (var element in glossariesLocales)
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
        /// Пересоздание связей глоссария с проектами локализации (Glossaries с LocalizationProjects).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <param name="localizationProjectsIds">Выбранные проекты локализации.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task EditGlossariesLocalizationProjectsAsync(int glossaryId, IEnumerable<int?> localizationProjectsIds, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryLocalizationProjectsGlossariesDelete = new Query("LocalizationProjectsGlossaries").Where("ID_Glossary", glossaryId).AsDelete();
                        var compiledQueryLocalizationProjectsGlossariesDelete = _compiler.Compile(queryLocalizationProjectsGlossariesDelete);
                        LogQuery(compiledQueryLocalizationProjectsGlossariesDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryLocalizationProjectsGlossariesDelete.Sql,
                            param: compiledQueryLocalizationProjectsGlossariesDelete.NamedBindings);
                    }

                    var localizationProjectsGlossaries = localizationProjectsIds.Select(t => new
                    {
                        ID_Glossary = glossaryId,
                        ID_LocalizationProject = t
                    }).ToList();

                    foreach (var element in localizationProjectsGlossaries)
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
                    //Таблицы с прямой ссылкой на "Glossaries" - "GlossariesStrings", "GlossariesLocales", "LocalizationProjectsGlossaries".
                    
                    //все из TranslationSubstrings и Files удаляется по Glossaries.ID_File

                    //Удаление зависимостей
                    #region Get Glossaries.ID_File

                    var queryGetGlossariesID_File = new Query("Glossaries").Where("ID", id).Select("Glossaries.ID_File");
                    var compiledQueryGetGlossariesID_File = _compiler.Compile(queryGetGlossariesID_File);
                    LogQuery(compiledQueryGetGlossariesID_File);
                    var idFile = await dbConnection.QueryFirstOrDefaultAsync<int?>(
                        sql: compiledQueryGetGlossariesID_File.Sql,
                        param: compiledQueryGetGlossariesID_File.NamedBindings);

                    #endregion

                    if (idFile != null)
                    {
                        #region Таблицы с прямой ссылкой на "Files" (Glossaries.ID_File) - "Tasks", "FilesLocales".

                        var queryUserActions = new Query("UserActions").Where("ID_File", idFile)
                            .AsUpdate(
                                new[] { "ID_File" },
                                new object[] { null }
                                );
                        var compiledQueryUserActions = _compiler.Compile(queryUserActions);
                        LogQuery(compiledQueryUserActions);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryUserActions.Sql,
                            param: compiledQueryUserActions.NamedBindings);

                        #region Таблицы с прямой ссылкой на "TranslationSubstrings" (Glossaries.ID_File -> TranslationSubstrings.ID_FileOwner => TranslationSubstrings.ID) - "StringsContextImages", "TranslationsubStringsLocales", "Translations", "Comments".

                        #region Get TranslationSubstrings.ID

                        var queryGetTranslationSubstringsID1 = new Query("TranslationSubstrings").Where("ID_FileOwner", idFile).Select("TranslationSubstrings.ID");
                        var compiledQueryGetTranslationSubstringsID1 = _compiler.Compile(queryGetTranslationSubstringsID1);
                        LogQuery(compiledQueryGetTranslationSubstringsID1);
                        var idsTranslationSubstrings1 = await dbConnection.QueryAsync<int>(
                            sql: compiledQueryGetTranslationSubstringsID1.Sql,
                            param: compiledQueryGetTranslationSubstringsID1.NamedBindings);

                        var queryGetTranslationSubstringsID2 = new Query("GlossariesStrings").Where("ID_Glossary", id).Select("GlossariesStrings.ID_String");
                        var compiledQueryGetTranslationSubstringsID2 = _compiler.Compile(queryGetTranslationSubstringsID2);
                        LogQuery(compiledQueryGetTranslationSubstringsID2);
                        var idsTranslationSubstrings2 = await dbConnection.QueryAsync<int>(
                            sql: compiledQueryGetTranslationSubstringsID2.Sql,
                            param: compiledQueryGetTranslationSubstringsID2.NamedBindings);

                        var idsTranslationSubstrings = new List<int>();
                        idsTranslationSubstrings.AddRange(idsTranslationSubstrings1);
                        idsTranslationSubstrings.AddRange(idsTranslationSubstrings2);

                        #endregion

                        if (idsTranslationSubstrings != null && idsTranslationSubstrings.Count() > 0)
                        {
                            var queryUserActions2 = new Query("UserActions").WhereIn("ID_String", idsTranslationSubstrings)
                                .AsUpdate(
                                    new[] { "ID_String" },
                                    new object[] { null }
                                    );
                            var compiledQueryUserActions2 = _compiler.Compile(queryUserActions2);
                            LogQuery(compiledQueryUserActions2);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryUserActions2.Sql,
                                param: compiledQueryUserActions2.NamedBindings);

                            #region Translations

                            var queryGetTranslationsID = new Query("Translations").WhereIn("ID_String", idsTranslationSubstrings).Select("Translations.ID");
                            var compiledQueryGetTranslationsID = _compiler.Compile(queryGetTranslationsID);
                            LogQuery(compiledQueryGetTranslationsID);
                            var idsTranslations = await dbConnection.QueryAsync<int>(
                                sql: compiledQueryGetTranslationsID.Sql,
                                param: compiledQueryGetTranslationsID.NamedBindings);

                            var queryUserActions3 = new Query("UserActions").WhereIn("ID_Translation", idsTranslations)
                                .AsUpdate(
                                    new[] { "ID_Translation" },
                                    new object[] { null }
                                    );
                            var compiledQueryUserActions3 = _compiler.Compile(queryUserActions3);
                            LogQuery(compiledQueryUserActions3);
                            await dbConnection.ExecuteAsync(
                                sql: compiledQueryUserActions3.Sql,
                                param: compiledQueryUserActions3.NamedBindings);

                            #endregion
                            
                            #region "TranslationSubstrings" (Glossaries.ID_File)

                            var queryTranslationSubstrings = new Query("TranslationSubstrings").WhereIn("ID", idsTranslationSubstrings).AsDelete();
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
                    var query = new Query("Glossaries").Where("ID", id).AsDelete();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);


                    //Удаление зависимостей
                    #region "Files" (Glossaries.ID_File)

                    var queryFiles = new Query("Files").Where("ID", idFile).AsDelete();
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

    }
}
