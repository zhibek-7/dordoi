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
                            //"Glossaries.Description",
                            //"Glossaries.ID_File",
                            "Locales.ID as LocaleID",
                            "Locales.Name as LocaleName",
                            "LocalizationProjects.ID as LocalizationProjectID",
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
        /// <param name="glossary">новый глоссарий</param>
        /// <returns></returns>
        public async Task AddNewGlossaryAsync(GlossariesForEditing glossary)
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
        /// Возвращает глоссарий для редактирования (без группировки по объектам) //(со связанными объектами)
        /// </summary>
        /// <param name="glossaryId">идентификатор глоссария</param>
        /// <returns></returns>
        public async Task<GlossariesForEditing> GetGlossaryForEditAsync(int glossaryId)
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
                    
                    //Перенести в service
                    var glossariesForEditing = new GlossariesForEditing
                    {
                        ID = glossaries.FirstOrDefault().ID,
                        Name = glossaries.FirstOrDefault().Name,
                        Description = glossaries.FirstOrDefault().Description,
                        ID_File = glossaries.FirstOrDefault().ID_File,
                        LocalesIds = glossaries.Select(t => t.LocaleID).Distinct(),
                        LocalizationProjectsIds = glossaries.Select(t => t.LocalizationProjectID).Distinct()
                    };
                    //

                    return glossariesForEditing;
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
        /// <param name="glossary">отредактированный глоссарий</param>
        /// <returns></returns>
        public async Task EditGlossaryAsync(GlossariesForEditing glossary)
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

                    #region GlossariesLocales

                    //Пересоздание связей глоссария с языками перевода (Glossaries с Locales)

                    var queryGlossariesLocalesDelete = new Query("GlossariesLocales").Where("ID_Glossary", glossary.ID).AsDelete();
                    var compiledQueryGlossariesLocalesDelete = _compiler.Compile(queryGlossariesLocalesDelete);
                    LogQuery(compiledQueryGlossariesLocalesDelete);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesLocalesDelete.Sql,
                        param: compiledQueryGlossariesLocalesDelete.NamedBindings);
                    var GlossariesLocales = glossary.LocalesIds.Select(t => new
                    {
                        ID_Glossary = glossary.ID,
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

                    #endregion

                    #region LocalizationProjectsGlossaries

                    //Пересоздание связей глоссария с проектами локализации (Glossaries с LocalizationProjects)

                    var queryLocalizationProjectsGlossariesDelete = new Query("LocalizationProjectsGlossaries").Where("ID_Glossary", glossary.ID).AsDelete();
                    var compiledQueryLocalizationProjectsGlossariesDelete = _compiler.Compile(queryLocalizationProjectsGlossariesDelete);
                    LogQuery(compiledQueryLocalizationProjectsGlossariesDelete);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryLocalizationProjectsGlossariesDelete.Sql,
                        param: compiledQueryLocalizationProjectsGlossariesDelete.NamedBindings);
                    var LocalizationProjectsGlossaries = glossary.LocalizationProjectsIds.Select(t => new
                    {
                        ID_Glossary = glossary.ID,
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

                    #endregion

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
        /// Удаление глоссария
        /// </summary>
        /// <param name="id">идентификатор глоссария</param>
        /// <returns></returns>
        public async Task DeleteGlossaryAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    #region
                    /*
                    при удалени глосария нужно удалить файл, строки, термины и переводы

                    все из TranslationSubstrings и Files удалю по Glossaries.ID_File                    
                    еще Translations (ID_String), на них CommentsImages(ID_Comment),Comments(ID_TranslationSubstrings)

                    констрейны нужно сделать, что бы автоматом
                    */


                    //var queryGlossariesStrings = new Query("GlossariesStrings").Where("ID_Glossary", id).AsDelete();
                    //var compiledQueryGlossariesStrings = _compiler.Compile(queryGlossariesStrings);
                    //LogQuery(compiledQueryGlossariesStrings);
                    //await dbConnection.ExecuteAsync(
                    //    sql: compiledQueryGlossariesStrings.Sql,
                    //    param: compiledQueryGlossariesStrings.NamedBindings
                    //);

                    //var queryTranslationSubstrings = new Query("TranslationSubstrings").Where("ID_FileOwner", fileId).AsDelete();
                    //var compiledQueryTranslationSubstrings = _compiler.Compile(queryTranslationSubstrings);
                    //LogQuery(compiledQueryTranslationSubstrings);
                    //await dbConnection.ExecuteAsync(
                    //    sql: compiledQueryTranslationSubstrings.Sql,
                    //    param: compiledQueryTranslationSubstrings.NamedBindings
                    //);
                    #endregion

                    //Удаление зависимостей
                    var queryGlossariesLocales = new Query("GlossariesLocales").Where("ID_Glossary", id).AsDelete();
                    var compiledQueryGlossariesLocales = _compiler.Compile(queryGlossariesLocales);
                    LogQuery(compiledQueryGlossariesLocales);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesLocales.Sql,
                        param: compiledQueryGlossariesLocales.NamedBindings);

                    var queryLocalizationProjectsGlossaries = new Query("LocalizationProjectsGlossaries").Where("ID_Glossary", id).AsDelete();
                    var compiledQueryLocalizationProjectsGlossaries = _compiler.Compile(queryLocalizationProjectsGlossaries);
                    LogQuery(compiledQueryLocalizationProjectsGlossaries);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryLocalizationProjectsGlossaries.Sql,
                        param: compiledQueryLocalizationProjectsGlossaries.NamedBindings);

                    //Удаление глоссария
                    var query = new Query("Glossaries").Where("ID", id).AsDelete();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
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
