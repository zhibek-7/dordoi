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
using Models.Migration;

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
            "\"Name\", " +
            "\"Description\", " +
            "\"Flag\", " +
            "\"code\", " +
            "\"data_create\", " +
            "\"url\", " +
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
                            "locales.id as LocaleId",
                            "locales.name_text as LocaleName",
                            "locales.url as LocaleUrl",

                            "localization_projects_locales.percent_of_translation",
                            "localization_projects_locales.percent_of_confirmed"
                            )
                        .OrderBy("LocaleName");
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

        public async Task<bool> Upload(Locale locale)
        {
            var sqlString = this._insertLocaleSql + " RETURNING id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        this.LogQuery(sqlString, locale);
                        var insertedId = await connection.ExecuteScalarAsync<int?>(sqlString, locale, transaction);
                        if (!insertedId.HasValue)
                        {
                            this._loggerError.WriteLn("Insertion into Locales didn't return id.");
                            transaction.Rollback();
                            return false;
                        }
                        locale.ID = insertedId.Value;
                        return true;
                    }
                    catch (NpgsqlException exception)
                    {
                        this._loggerError.WriteLn(
                            $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.Upload)} {nameof(NpgsqlException)} ",
                            exception);
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception exception)
                    {
                        this._loggerError.WriteLn(
                            $"Ошибка в {nameof(LocaleRepository)}.{nameof(LocaleRepository.Upload)} {nameof(Exception)} ",
                            exception);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}

