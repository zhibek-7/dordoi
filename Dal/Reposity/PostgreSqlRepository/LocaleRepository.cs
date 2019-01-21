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
{
    public class LocaleRepository : BaseRepository, ILocaleRepository
    {
        public LocaleRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<IEnumerable<Locale>> GetAllAsync()
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>("SELECT * FROM \"Locales\"");
                return users;
            }
        }

        public async Task<IEnumerable<Locale>> GetAllForProject(int projectId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>(
                    "SELECT l.* FROM \"Locales\" l " +
                    " join \"LocalizationProjectsLocales\" pl on pl.\"ID_Locale\" = l.\"ID\" " +
                    " join \"LocalizationProjects\" lp on pl.\"ID_LocalizationProject\" = lp.\"ID\" " +
                    " where lp.\"ID\" = @Id", new { Id = projectId });
                return users;
            }
        }


        public async Task<IEnumerable<Locale>> GetByUserIdAsync(int userId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
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
                return userLocales;
            }
        }

    }
}

