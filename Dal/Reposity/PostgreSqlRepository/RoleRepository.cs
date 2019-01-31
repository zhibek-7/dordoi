using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class RoleRepository : BaseRepository, IBaseRepositoryAsync<Role>
    {

        public RoleRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Roles");
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var roles = await dbConnection.QueryAsync<Role>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return roles;
                }
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(RoleRepository)}.{nameof(RoleRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

    }
}
