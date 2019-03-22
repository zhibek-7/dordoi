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
using System.Data;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class TranslationMemoryRepository : BaseRepository, ITranslationMemoryRepository
    {
        private FilesRepository fr;
        private ProjectTranslationMemoryRepository _translationMemoryRepository;
        public TranslationMemoryRepository(string connectionStr) : base(connectionStr)
        {
            fr = new FilesRepository(connectionStr);
            _translationMemoryRepository = new ProjectTranslationMemoryRepository(connectionStr);
        }


        public static Dictionary<string, string> SortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "translation_memories.id" },
            { "name_text", "translation_memories.name_text" },
            { "locales_name", "locales_tm.locales_name"},//"locales.name_text" },
            { "localization_projects_name", "localization_projects_tm.localization_projects_name"},//"localization_projects.name_text" },
            { "string_count", "string_count" }
        };


        /// <summary>
        /// Возвращает памяти переводов (со связанными объектами).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationMemoryTableViewDTO>> GetAllByUserIdAsync(
            Guid? userId,
            int offset,
            int limit,
            Guid? projectId = null,
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
                        columnNamesMappings: TranslationMemoryRepository.SortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var compiledQuery = this._compiler.Compile(query);
                    LogQuery(compiledQuery);

                    var translationMemories = await dbConnection.QueryAsync<TranslationMemoryTableViewDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );

                    return translationMemories;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllByUserIdAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllByUserIdAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает количество памятей переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
        /// <returns></returns>
        public async Task<int?> GetAllByUserIdCountAsync(
            Guid? userId,
            Guid? projectId = null,
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
                    query = query.Distinct().AsCount("translation_memories.id");


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
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllByUserIdCountAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllByUserIdCountAsync)} {nameof(Exception)} ", exception);
                return null;
            }

        }

        /// <summary>
        /// Возвращает запрос памятей переводов (со связанными объектами).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
        /// <returns></returns>
        private Query GetAllByUserIdQuery(
            Guid? userId,
            Guid? projectId = null,
            string searchString = null)
        {
            try
            {
                var queryLocalesTranslationMemories = new Query("translation_memories")
                    .LeftJoin("translation_memories_locales", "translation_memories_locales.id_translation_memory", "translation_memories.id")
                    .LeftJoin("locales", "locales.id", "translation_memories_locales.id_locale")
                    .Select("translation_memories.id as tm_id")
                    .GroupBy("translation_memories.id")
                    .SelectRaw("string_agg(locales.name_text, ', ' order by locales.name_text) as locales_name");

                var queryLocalizationProjectsTranslationMemories = new Query("translation_memories")
                    .LeftJoin("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
                    .LeftJoin("localization_projects", "localization_projects.id", "localization_projects_translation_memories.id_localization_project")
                    .Join("participants", "participants.id_localization_project", "localization_projects.id")
                    .WhereTrue("participants.active")
                    .Where("participants.id_user", (Guid)userId)
                    .Select("translation_memories.id as tm_id")
                    .GroupBy("translation_memories.id")
                    .SelectRaw("string_agg(localization_projects.name_text, ', ' order by localization_projects.name_text) as localization_projects_name");


                var query = new Query("translation_memories")
                    .With("locales_tm", queryLocalesTranslationMemories)
                    .With("localization_projects_tm", queryLocalizationProjectsTranslationMemories)
                    .LeftJoin("translation_memories_strings", "translation_memories_strings.id_translation_memory", "translation_memories.id")
                    .Join("locales_tm", "locales_tm.tm_id", "translation_memories.id")
                    .Join("localization_projects_tm", "localization_projects_tm.tm_id", "translation_memories.id")
                    .Select(
                        "translation_memories.id",
                        "translation_memories.name_text",
                        "locales_tm.locales_name",
                        "localization_projects_tm.localization_projects_name"
                    )
                    .SelectRaw("COUNT(translation_memories_strings.id_translation_memory) AS string_count")
                    .GroupBy("translation_memories.id",
                        "translation_memories.name_text",
                        "locales_tm.locales_name",
                        "localization_projects_tm.localization_projects_name");

                if (projectId != null)
                {
                    query = query
                        .Join("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
                        .Where("localization_projects_translation_memories.id_localization_project", projectId);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchPattern = $"%{searchString}%";
                    query = query.WhereLike("translation_memories.name_text", searchPattern);
                }

                var compiledQuery = _compiler.Compile(query);
                LogQuery(compiledQuery);

                return query;
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllByUserIdQuery)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetAllByUserIdQuery)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<IEnumerable<TranslationMemory>> GetAllAsync(Guid? userId, Guid? projectId)
        {
            return null;
        }

        /// <summary>
        /// Возвращает список памятей переводов назначенных на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns>TranslationMemoryForSelectDTO</returns>
        public async Task<IEnumerable<TranslationMemoryForSelectDTO>> GetForSelectByProjectAsync(Guid projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_memories")
                        .Join("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
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
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetForSelectByProjectAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(TranslationMemoryRepository.GetForSelectByProjectAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает память переводов для редактирования (без группировки по объектам).
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationMemory>> GetForEditAsync(Guid translationMemoryId)
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
        /// Добавление новой памяти переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemory">Новая память переводов.</param>
        /// <returns></returns>
        public async Task AddAsync(Guid userId, TranslationMemoryForEditingDTO translationMemory)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    dbConnection.Open();
                    using (IDbTransaction transaction = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        //Добавление памяти
                        var newGlossaryFileId = await this.fr.AddAsync(new File()
                        {
                            id_localization_project = null,
                            name_text = translationMemory.name_text,
                            date_of_change = DateTime.Now,
                            is_folder = false,
                            is_last_version = true,
                            visibility = true
                        }, dbConnection, transaction);
                        translationMemory.id_file = newGlossaryFileId;


                        //Добавление новой памяти переводов
                        var newTranslationMemory = new
                        {
                            translationMemory.name_text,
                            translationMemory.id_file
                        };
                        /*
                        var query = new Query("translation_memories")
                            .AsInsert(newTranslationMemory, true); //true - вернуть сгенерированный id нового объекта
                        var compiledQuery = _compiler.Compile(query);
                        LogQuery(compiledQuery);
    
                        //После выполнение запроса получаем сгенерированный id нового объекта
                        var idOfNewTranslationMemory = await dbConnection
                            .ExecuteScalarAsync<Guid>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
                        */

                        var sql = "INSERT INTO translation_memories(name_text, id_file) VALUES('" +
                                  translationMemory.name_text + "', '" + translationMemory.id_file +
                                  "') RETURNING  translation_memories.id";
                        LogQuery(sql);
                        var idOfNewTranslationMemory = await dbConnection.ExecuteScalarAsync<Guid>(sql, transaction);


                        //Добавление в таблицу "translation_memories_locales" записей связи памяти переводов с языками перевода (translation_memories с locales)
                        await UpdateTranslationMemoriesLocalesAsync(idOfNewTranslationMemory,
                            ConvertData.ConverLocale(translationMemory.locales_ids), dbConnection, transaction, false);

                        //Добавление в таблицу "localization_projects_translation_memories" записей связи памяти переводов с проектами локализации (translation_memories с localization_projects)
                        await _translationMemoryRepository.UpdateTranslationMemoriesLocalizationProjectsAsync(userId,
                            idOfNewTranslationMemory, translationMemory.localization_projects_ids, dbConnection, transaction, false);
                        transaction.Commit();
                    }
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
        /// Сохранение изменений в памяти переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemory">Отредактированная память переводов.</param>
        /// <returns></returns>
        public async Task UpdateAsync(Guid userId, TranslationMemoryForEditingDTO translationMemory)
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

                    //TODO транзакции

                    //TODO транзакции
                    //Пересоздание в таблице "translation_memories_locales" записей связи памяти переводов с языками перевода (translation_memories с locales)
                    await UpdateTranslationMemoriesLocalesAsync(translationMemory.id, ConvertData.ConverLocale(translationMemory.locales_ids), dbConnection, null, true);
                    //TODO транзакции
                    //Пересоздание в таблице "localization_projects_translation_memories" записей связи памяти переводов с проектами локализации (translation_memories с localization_projects)
                    await _translationMemoryRepository.UpdateTranslationMemoriesLocalizationProjectsAsync(userId, translationMemory.id, translationMemory.localization_projects_ids);

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
        public async Task UpdateTranslationMemoriesLocalesAsync(Guid translationMemoryId, IEnumerable<Guid> localesIds, NpgsqlConnection connection, IDbTransaction transaction, bool isDeleteOldRecords = true)
        {
            try
            {
                // using (var connection = new NpgsqlConnection(connectionString))
                {
                    //connection.Open();
                    //using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        if (isDeleteOldRecords)
                        {
                            var queryDelete = new Query("translation_memories_locales")
                                .Where("id_translation_memory", translationMemoryId)
                                .AsDelete();
                            var compiledQueryDelete = _compiler.Compile(queryDelete);
                            LogQuery(compiledQueryDelete);
                            await connection.ExecuteAsync(
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
                            await connection.ExecuteAsync(
                                sql: compiledQueryInsert.Sql,
                                param: compiledQueryInsert.NamedBindings);
                        }


                        ///обновляем у  файла локали
                        Guid? fileId = GetIdFile(translationMemoryId, connection).Result;
                        await this.fr.DeleteTranslationLocalesAsync(fileId: (Guid)fileId);
                        await this.fr.AddTranslationLocalesAsync(fileId: (Guid)fileId, localesIds: localesIds,
                            dbConnection: connection, transaction: transaction);

                        //transaction.Commit();
                    }
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
        private async Task<Guid?> GetIdFile(Guid id, NpgsqlConnection dbConnection)
        {
            //Удаление зависимостей


            var queryGetGlossariesID_File = new Query("translation_memories").Where("id", id).Select("translation_memories.id_file");
            var compiledQueryGetGlossariesID_File = _compiler.Compile(queryGetGlossariesID_File);
            LogQuery(compiledQueryGetGlossariesID_File);
            var idFile = await dbConnection.QueryFirstOrDefaultAsync<Guid?>(
                sql: compiledQueryGetGlossariesID_File.Sql,
                param: compiledQueryGetGlossariesID_File.NamedBindings);
            return idFile;
        }

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Guid id)
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
                    var idsTranslationSubstrings = await dbConnection.QueryAsync<Guid>(
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
                    var idFile = await dbConnection.QueryFirstOrDefaultAsync<Guid?>(
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
