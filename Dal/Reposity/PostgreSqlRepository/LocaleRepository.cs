using System;
using System.Collections.Generic;
using Models.DatabaseEntities;
using Dapper;
using System.Data;
using System.Linq;
using DAL.Context;
using System.Threading.Tasks;
using SqlKata;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{ /// <summary>
/// ///нужно ли здесь ставить  this.LogQuery(sqlString); не поняла, так как в filerepository в примерно таких запросах  this.LogQuery(sqlString); нет
/// </summary>
    public class LocaleRepository : BaseRepository
    {
        private PostgreSqlNativeContext context;

        public LocaleRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public async Task<IEnumerable<Locale>> GetAllAsync()
        {
            try {

                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>("SELECT * FROM \"Locales\"");
                    dbConnection.Close();
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
            try {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>(
                        "SELECT l.* FROM \"Locales\" l " +
                        " join \"LocalizationProjectsLocales\" pl on pl.\"ID_Locale\" = l.\"ID\" " +
                        " join \"LocalizationProjects\" lp on pl.\"ID_LocalizationProject\" = lp.\"ID\" " +
                        " where lp.\"ID\" = @Id", new { Id = projectId });
                    dbConnection.Close();
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


        public async Task<IEnumerable<Locale>> GetByUserIdAsync(int userId)
        {
          try {
                using (var dbConnection = this.context.Connection)
                {
                    dbConnection.Open();
                    var query =
                        new Query("Locales")
                        .WhereIn("ID",
                            new Query("UsersLocales")
                            .Select("ID_Locale")
                            .Where("ID_User", userId));
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var userLocales = await dbConnection.QueryAsync<Locale>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    dbConnection.Close();
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

    }
}

