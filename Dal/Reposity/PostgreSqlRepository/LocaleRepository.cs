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

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocaleRepository : BaseRepository
    {
        private PostgreSqlNativeContext context;

        public LocaleRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public async Task<IEnumerable<Locale>> GetAllAsync()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>("SELECT * FROM \"Locales\"");
                dbConnection.Close();
                return users;
            }
        }

        public async Task<IEnumerable<Locale>> GetAllForProject(int projectId)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Locale> users = await dbConnection.QueryAsync<Locale>(
                    "SELECT l.* FROM \"Locales\" l " +
                    " join \"LocalizationProjectsLocales\" pl on pl.\"ID_Locale\" = l.\"ID\" " +
                    " join \"LocalizationProjects\" lp on pl.\"ID_LocalizationProject\" = lp.\"ID\" " +
                    " where lp.\"ID\" = @Id", new { projectId });
                dbConnection.Close();
                return users;
            }
        }


        public async Task<IEnumerable<Locale>> GetByUserIdAsync(int userId)
        {
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

    }
}

