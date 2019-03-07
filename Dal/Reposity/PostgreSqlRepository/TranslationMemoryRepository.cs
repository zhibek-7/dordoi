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

namespace DAL.Reposity.PostgreSqlRepository
{
    public class TranslationMemoryRepository : BaseRepository, ITranslationMemoryRepository
    {
        private FilesRepository fr;
        public TranslationMemoryRepository(string connectionStr) : base(connectionStr)
        {
            fr = new FilesRepository(connectionStr);
        }


        /// <summary>
        /// Возвращает все строки запроса (без группировки по объектам).
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationMemory>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_memories")
                        .LeftJoin("translation_memories_locales", "translation_memories_locales.id_translation_memory", "translation_memories.id")
                        .LeftJoin("locales", "locales.id", "translation_memories_locales.id_locale")
                        .LeftJoin("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
                        .LeftJoin("localization_projects", "localization_projects.id", "localization_projects_translation_memories.id_localization_project")
                        .LeftJoin("translation_memories_strings", "translation_memories_strings.id_translation_memory", "translation_memories.id")
                        .Select(
                            "translation_memories.id",
                            "translation_memories.name_text",
                            "locales.name_text as locale_name",
                            "localization_projects.name_text as localization_project_name"//,
                                                                                          //"COUNT(translation_memories_strings.id_translation_memory) AS string_count"
                        )
                        .SelectRaw("COUNT(translation_memories_strings.id_translation_memory) AS string_count")
                        .GroupBy("translation_memories.id",
                            "translation_memories.name_text",
                            "locales.name_text",
                            "localization_projects.name_text");
                    //.Select(countQuery, "string_count");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var translationMemories = await dbConnection.QueryAsync<TranslationMemory>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return translationMemories;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает список памятей переводов назначенных на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns>TranslationMemoryForSelectDTO</returns>
        public async Task<IEnumerable<TranslationMemoryForSelectDTO>> GetForSelectByProjectAsync(int projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_memories")
                        .LeftJoin("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
                        .Where("localization_projects_translation_memories.id_localization_project", projectId)
                        .Select("translation_memories.id",
                                "translation_memories.name_text");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var translationMemories = await dbConnection.QueryAsync<TranslationMemoryForSelectDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return translationMemories;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Добавление новой памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Новая память переводов.</param>
        /// <returns></returns>
        public async Task AddAsync(TranslationMemoryForEditingDTO translationMemory)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Добавление новой памяти переводов
                    var newTranslationMemory = new
                    {
                        translationMemory.name_text,
                        translationMemory.id_file
                    };
                    var query = new Query("translation_memories")
                        .AsInsert(newTranslationMemory, true); //true - вернуть сгенерированный id нового объекта
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    //После выполнение запроса получаем сгенерированный id нового объекта
                    var idOfNewTranslationMemory = await dbConnection
                        .ExecuteScalarAsync<int>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);

                    //Добавление в таблицу "translation_memories_locales" записей связи памяти переводов с языками перевода (translation_memories с locales)
                    await UpdateTranslationMemoriesLocalesAsync(idOfNewTranslationMemory, ConvertData.ConverLocale(translationMemory.locales_ids), false);

                    //Добавление в таблицу "localization_projects_translation_memories" записей связи памяти переводов с проектами локализации (translation_memories с localization_projects)
                    await UpdateTranslationMemoriesLocalizationProjectsAsync(idOfNewTranslationMemory, translationMemory.localization_projects_ids, false);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.AddAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.AddAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Возвращает память переводов для редактирования (без группировки по объектам).
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationMemory>> GetForEditAsync(int translationMemoryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_memories").Where("translation_memories.id", translationMemoryId)
                        .LeftJoin("translation_memories_locales", "translation_memories_locales.id_translation_memory", "translation_memories.id")
                        .LeftJoin("locales", "locales.id", "translation_memories_locales.id_locale")
                        .LeftJoin("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
                        .LeftJoin("localization_projects", "localization_projects.id", "localization_projects_translation_memories.id_localization_project")
                        .Select(
                            "translation_memories.id",
                            "translation_memories.name_text",
                            "translation_memories.id_file",
                            "locales.id as locale_id",
                            "localization_projects.id as localization_project_id"
                        );
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var translationMemories = await dbConnection.QueryAsync<TranslationMemory>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return translationMemories;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetForEditAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetForEditAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Сохранение изменений в памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Отредактированная память переводов.</param>
        /// <returns></returns>
        public async Task UpdateAsync(TranslationMemoryForEditingDTO translationMemory)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Обновление памяти переводов
                    var editedTranslationMemory = new
                    {
                        translationMemory.name_text,
                        translationMemory.id_file
                    };
                    var query = new Query("translation_memories")
                        .Where("id", translationMemory.id)
                        .AsUpdate(editedTranslationMemory);
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);


                    //Пересоздание в таблице "translation_memories_locales" записей связи памяти переводов с языками перевода (translation_memories с locales)
                    await UpdateTranslationMemoriesLocalesAsync(translationMemory.id, ConvertData.ConverLocale(translationMemory.locales_ids));

                    //Пересоздание в таблице "localization_projects_translation_memories" записей связи памяти переводов с проектами локализации (translation_memories с localization_projects)
                    await UpdateTranslationMemoriesLocalizationProjectsAsync(translationMemory.id, translationMemory.localization_projects_ids);

                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.UpdateAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.UpdateAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Пересоздание в таблице "translation_memories_locales" связей памяти переводов с языками перевода (translation_memories с locales).
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="localesIds">Выбранные языки перевода.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task UpdateTranslationMemoriesLocalesAsync(int translationMemoryId, IEnumerable<int> localesIds, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryDelete = new Query("translation_memories_locales")
                            .Where("id_translation_memory", translationMemoryId)
                            .AsDelete();
                        var compiledQueryDelete = _compiler.Compile(queryDelete);
                        LogQuery(compiledQueryDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryDelete.Sql,
                            param: compiledQueryDelete.NamedBindings);
                    }

                    var translationMemoriesLocales = localesIds.Select(t => new
                    {
                        id_translation_memory = translationMemoryId,
                        id_locale = t
                    }).ToList();

                    foreach (var element in translationMemoriesLocales)
                    {
                        var queryInsert = new Query("translation_memories_locales")
                            .AsInsert(element);
                        var compiledQueryInsert = _compiler.Compile(queryInsert);
                        LogQuery(compiledQueryInsert);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryInsert.Sql,
                                param: compiledQueryInsert.NamedBindings);
                    }


                    ///обновляем у  файла локали
                    int? fileId = GetIdFile(translationMemoryId, dbConnection).Result;
                    await this.fr.DeleteTranslationLocalesAsync(fileId: (int)fileId);
                    await this.fr.AddTranslationLocalesAsync(fileId: (int)fileId, localesIds: localesIds);

                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.UpdateTranslationMemoriesLocalesAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.UpdateTranslationMemoriesLocalesAsync)} {nameof(Exception)} ", exception);
            }
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


            var queryGetGlossariesID_File = new Query("translation_memories").Where("id", id).Select("translation_memories.id_file");
            var compiledQueryGetGlossariesID_File = _compiler.Compile(queryGetGlossariesID_File);
            LogQuery(compiledQueryGetGlossariesID_File);
            var idFile = await dbConnection.QueryFirstOrDefaultAsync<int?>(
                sql: compiledQueryGetGlossariesID_File.Sql,
                param: compiledQueryGetGlossariesID_File.NamedBindings);
            return idFile;
        }

        /// <summary>
        /// Пересоздание в таблице "localization_projects_translation_memories" связей памяти переводов с проектами локализации (translation_memories с localization_projects).
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="localizationProjectsIds">Выбранные проекты локализации.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task UpdateTranslationMemoriesLocalizationProjectsAsync(int translationMemoryId, IEnumerable<int?> localizationProjectsIds, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryDelete = new Query("localization_projects_translation_memories")
                            .Where("id_translation_memory", translationMemoryId)
                            .AsDelete();
                        var compiledQueryDelete = _compiler.Compile(queryDelete);
                        LogQuery(compiledQueryDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryDelete.Sql,
                            param: compiledQueryDelete.NamedBindings);
                    }

                    var localizationProjectsTranslationMemories = localizationProjectsIds.Select(t => new
                    {
                        id_translation_memory = translationMemoryId,
                        id_localization_project = t
                    }).ToList();

                    foreach (var element in localizationProjectsTranslationMemories)
                    {
                        var queryInsert = new Query("localization_projects_translation_memories").AsInsert(element);
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
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.UpdateTranslationMemoriesLocalizationProjectsAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.UpdateTranslationMemoriesLocalizationProjectsAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //правилом каскадного удаления в БД удаляются зависимые данные


                    var queryGetTranslationSubstringsId = new Query("translation_memories_strings")
                        .Where("id_translation_memory", id)
                        .Select("translation_memories_strings.id_string");
                    var compiledQueryGetTranslationSubstringsId = _compiler.Compile(queryGetTranslationSubstringsId);
                    LogQuery(compiledQueryGetTranslationSubstringsId);
                    var idsTranslationSubstrings = await dbConnection.QueryAsync<int>(
                        sql: compiledQueryGetTranslationSubstringsId.Sql,
                        param: compiledQueryGetTranslationSubstringsId.NamedBindings);

                    if (idsTranslationSubstrings != null && idsTranslationSubstrings.Count() > 0)
                    {
                        var queryTranslationSubstrings = new Query("translation_substrings")
                            .WhereIn("id", idsTranslationSubstrings).AsDelete();
                        var compiledQueryTranslationSubstrings = _compiler.Compile(queryTranslationSubstrings);
                        LogQuery(compiledQueryTranslationSubstrings);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryTranslationSubstrings.Sql,
                            param: compiledQueryTranslationSubstrings.NamedBindings);

                    }


                    //Удаление памяти переводов
                    var query = new Query("translation_memories")
                        .Where("id", id)
                        .AsDelete();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);


                    //Удаление зависимостей

                    var queryGetID_File = new Query("translation_memories")
                        .Where("id", id)
                        .Select("translation_memories.id_file");
                    var compiledQueryGetID_File = _compiler.Compile(queryGetID_File);
                    LogQuery(compiledQueryGetID_File);
                    var idFile = await dbConnection.QueryFirstOrDefaultAsync<int?>(
                        sql: compiledQueryGetID_File.Sql,
                        param: compiledQueryGetID_File.NamedBindings);

                    var queryFiles = new Query("files")
                        .Where("id", idFile)
                        .AsDelete();
                    var compiledQueryFiles = _compiler.Compile(queryFiles);
                    LogQuery(compiledQueryFiles);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryFiles.Sql,
                        param: compiledQueryFiles.NamedBindings);


                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.DeleteAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.DeleteAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }


    }
}
