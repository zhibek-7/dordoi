using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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


        public async Task<IEnumerable<Role>> GetAsync(Guid? userId, string nameRole)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("roles").WhereNot("roles.short", nameRole);
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

        public async Task<IEnumerable<Role>> GetAllAsync(Guid? userId, Guid? projectId)
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
        public Guid? GetRoleId(string shortName)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sqlString = "SELECT id FROM public.roles where short =  '" + shortName + "'";
                    this.LogQuery(sqlString);
                    var idRole = dbConnection.ExecuteScalar<Guid>(sqlString);

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
