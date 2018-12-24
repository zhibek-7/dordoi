using System;
using System.Collections.Generic;
using Models.DatabaseEntities;
using Dapper;
using System.Data;
using System.Linq;
using DAL.Context;
using System.Threading.Tasks;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocaleRepository : BaseRepository, IRepository<Locale>
    {

        private PostgreSqlNativeContext context;

        public LocaleRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(Locale locale)
        {
            throw new NotImplementedException();
        }

        public Locale GetByID(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Locale> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Locale> users = dbConnection.Query<Locale>("SELECT * FROM \"Locales\"").ToList();
                dbConnection.Close();
                return users;
            }
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Locale user)
        {
            throw new NotImplementedException();
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

