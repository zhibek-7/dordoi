using System;
using System.Collections.Generic;
using Models.DatabaseEntities;
using Dapper;
using System.Data;
using System.Linq;
using DAL.Context;
using System.Threading.Tasks;
using Models.DatabaseEntities.DTO;
using SqlKata;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocaleRepository : BaseRepository, ILocaleRepository
    {
        private readonly string _insertLocaleSql =
            "INSERT INTO locales (" +
            "name_text, " +
            "description, " +
            "flag, " +
            "code, " +
            "data_create, " +
            "url, " +
            ") " +
            "VALUES (" +
            "@Name, " +
            "@Description, " +
            "@Flag, " +
            "@code, " +
            "@data_create, " +
            "@url" +
            ")";

        public LocaleRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<IEnumerable<Locale>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sqlString = "SELECT * FROM locales";
                    this.LogQuery(sqlString);
                    IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>(sqlString);
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                            $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                            exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

        public async Task<IEnumerable<Locale>> GetAllForProject(int projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = "SELECT l.* FROM locales l " +
       " join localization_projects_locales pl on pl.id_locale = l.id " +
       " join localization_projects lp on pl.id_localization_project = lp.id " +
       " where lp.id = @Id";

                    var param = new { Id = projectId };
                    this.LogQuery(query, param);

                    IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>(query, param);
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetAllForProject)} {nameof(NpgsqlException)} ",
                        exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetAllForProject)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает назначенные языки перевода на проект локализации с процентами переводов по ним.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        public async Task<IEnumerable<LocalizationProjectsLocalesDTO>> GetAllForProjectWithPercent(int projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects_locales")
                        .Where("id_localization_project", projectId)
                        .LeftJoin("locales", "locales.id", "localization_projects_locales.id_locale")
                        .Select(
                            "locales.id as Locale_Id",
                            "locales.name_text as Locale_Name",
                            "locales.url as Locale_Url",

                            "localization_projects_locales.percent_of_translation",
                            "localization_projects_locales.percent_of_confirmed"
                            )
                        .OrderBy("Locale_Name");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var result = await dbConnection.QueryAsync<LocalizationProjectsLocalesDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return result;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetAllForProjectWithPercent)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetAllForProjectWithPercent)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }


        public async Task<IEnumerable<Locale>> GetByUserIdAsync(int userId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {


                    var query =
                        new Query("locales")
                        .WhereIn("id",
                            new Query("users_locales")
                            .Select("id_locale")
                            .Where("id_user", userId));
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var userLocales = await dbConnection.QueryAsync<Locale>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return userLocales;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByUserIdAsync)} {nameof(NpgsqlException)} ",
                        exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByUserIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<bool> AddAsync(Locale newLocale)
        {
            var sqlString = this._insertLocaleSql + " RETURNING id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    this.LogQuery(sqlString, newLocale);
                    var insertedId = await connection.ExecuteScalarAsync<int?>(sqlString, newLocale);
                    if (!insertedId.HasValue)
                    {
                        this._loggerError.WriteLn("Insertion into Locales didn't return id.");
                        return false;
                    }
                    newLocale.id = insertedId.Value;
                    return true;
                }
                catch (NpgsqlException exception)
                {
                    this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.AddAsync)} {nameof(NpgsqlException)} ",
                        exception);
                    return false;
                }
                catch (Exception exception)
                {
                    this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.AddAsync)} {nameof(Exception)} ",
                        exception);
                    return false;
                }
            }
        }

        public async Task CleanTableAsync()
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query =
                    new Query("Locales")
                    .AsDelete();
                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);
                var userLocales = await dbConnection.ExecuteAsync(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);
            }
        }

        /// <summary>
        /// Список языков назначенных на проекты, которые назначены на пользователя
        /// </summary>
        /// <param name="userName">логин пользователя</param>
        /// <returns></returns>
        public async Task<IEnumerable<Locale>> GetByUserProjectsAsync(string userName)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("locales")
                        .WhereIn("id",
                            new Query("users")
                                .LeftJoin("participants", "participants.id_user", "users.id")
                                .LeftJoin("localization_projects_locales", "localization_projects_locales.id_localization_project", "participants.id_localization_project")
                                .Where("users.name_text", userName)
                                .Select("localization_projects_locales.id_locale")
                            );
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var projectsLocales = await dbConnection.QueryAsync<Locale>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return projectsLocales;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByUserIdAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByUserIdAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает список языков назначенных на память переводов.
        /// </summary>
        /// <param name="idTranslationMemory">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Locale>> GetByTranslationMemory(int idTranslationMemory)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("locales")
                    .WhereIn("id",
                        new Query("translation_memories_locales")
                            .Where("translation_memories_locales.id_translation_memory", idTranslationMemory)
                            .Select("translation_memories_locales.id_locale")
                    )

                    .OrWhereIn("id",
                    new Query("localization_projects_translation_memories")
                        .LeftJoin("localization_projects", "localization_projects.id", "localization_projects_translation_memories.id_localization_project")
                        .Where("localization_projects_translation_memories.id_translation_memory", idTranslationMemory)
                        .Select("localization_projects.id_source_locale")
                    );
                    
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var translationMemoriesLocales = await dbConnection.QueryAsync<Locale>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return translationMemoriesLocales;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByTranslationMemory)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByTranslationMemory)} {nameof(Exception)} ", exception);
                return null;
            }
        }
        
        /// <summary>
        /// Возвращает список языков назначенных на строки.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <param name="idsTranslationSubstring">Идентификаторы строк.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Locale>> GetByIdsTranslationSubstring(int projectId, IEnumerable<int> idsTranslationSubstring)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("locales")
                    .WhereIn("id",
                        new Query("translation_substrings_locales")
                            .WhereIn("translation_substrings_locales.id_translation_substrings", idsTranslationSubstring)
                            .Select("translation_substrings_locales.id_locale")
                    )

                    .OrWhereIn("id",
                            new Query("translation_memories_strings")
                                //.LeftJoin("translation_memories_strings", "translation_memories_strings.id_string", "translation_substrings.id")
                                .LeftJoin("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories_strings.id_translation_memory")
                                .LeftJoin("localization_projects", "localization_projects.id", "localization_projects_translation_memories.id_localization_project")
                                .Where("localization_projects.id", projectId)
                                .WhereIn("translation_memories_strings.id_string", idsTranslationSubstring)
                                .Select("localization_projects.id_source_locale")
                    );

                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var translationSubstringsLocales = await dbConnection.QueryAsync<Locale>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return translationSubstringsLocales;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByIdsTranslationSubstring)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByIdsTranslationSubstring)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает основной язык проекта.
        /// </summary>
        /// <param name="id">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        public async Task<Locale> GetSourceLocaleLocalizationProject(int idProject)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects")
                        .Where("localization_projects.id", idProject)
                        .LeftJoin("locales", "locales.id", "localization_projects.id_source_locale")
                        .Select("locales.id", "locales.name_text");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var project = await dbConnection.QueryFirstOrDefaultAsync<Locale>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetSourceLocaleLocalizationProject)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetSourceLocaleLocalizationProject)} {nameof(Exception)} ", exception);
                return null;
            }
        }





        public Locale GetByID(int Id)
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM locales WHERE id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id };
                    this.LogQuery(sqlString, param);
                    var project = dbConnection.Query<Locale>(sqlString, param).FirstOrDefault();
                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByID)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.GetByID)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }


    }
}

