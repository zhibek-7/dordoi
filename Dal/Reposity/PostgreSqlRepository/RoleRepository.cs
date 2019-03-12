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

        public async Task<IEnumerable<Role>> GetAllAsync(int? userId, int? projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("roles");
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


        /// <summary>
        /// Возврощаем Id по сокращенному имени роли observer
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="shortName"></param>
        /// <returns></returns>
        public int? GetRoleId(string shortName)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sqlString = "SELECT id FROM public.roles where short =  '" + shortName + "'";
                    this.LogQuery(sqlString);
                    var idRole = dbConnection.ExecuteScalar<int>(sqlString);

                    return idRole;

                }
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(RoleRepository)}.{nameof(RoleRepository.GetRoleId)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

    }
}
