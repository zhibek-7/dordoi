using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Dapper;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class FundRepository : BaseRepository, IRepositoryAuthorizeAsync<Fund>
    {
        public FundRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<Guid?> CreateAsync(FundDTO fund)
        {
            var sqlQuery = "INSERT INTO fund " +
                            "(name_text, description, id_user "
                            + ") " +
                            "VALUES('"
                  + fund.name_text + "','" + fund.description + "','" + fund.id_user + "'"
                  + ")"
                   + " RETURNING fund.id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    this.LogQuery(sqlQuery);
                    Guid? fundId = await dbConnection.ExecuteScalarAsync<Guid>(sqlQuery, fund);
                    return (Guid)fundId;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.AddNewFundAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.AddNewFundAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public Task<Guid?> AddAsync(Fund item)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid?> AddNewFundAsync(FundDTO fund)
        {
            var sqlQuery = "INSERT INTO fund " +
                           "(name_text, description "
                           +") " +
                           "VALUES('"
                 + fund.name_text + "','" + fund.description + "'" 
                 +  ")"
                  + " RETURNING fund.id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    this.LogQuery(sqlQuery);
                    Guid? fundId = await dbConnection.ExecuteScalarAsync<Guid>(sqlQuery, fund);
                    return (Guid)fundId;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.AddNewFundAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.AddNewFundAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

       public Task<Guid?> AddNewFundAsync(Guid glossaryId, Guid? partOfSpeechId)
       {
       throw new NotImplementedException();
       }

        public async Task<IEnumerable<Fund>> GetAllAsync(Guid? userId, Guid? fundId)
        {
            // Sql string to select all rows where id_user = '" + (Guid)userId + @"'
            var sqlString = @"SELECT id, name_text, date_time_added, id_user, description
            FROM fund 
            
            order by name_text";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlString);
                    IEnumerable<Fund> users = dbConnection.Query<Fund>(sqlString);
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<Fund> GetByIDAsync(Guid id, Guid? userId)
        {
            var sqlString = @"SELECT id, name_text, date_time_added, id_user, description
            FROM fund 
            where id_user = '" + (Guid)userId + @"'
            order by name_text";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = id, @userId = userId };
                    this.LogQuery(sqlString, param);
                    var fund = dbConnection.Query<Fund>(sqlString, param).FirstOrDefault();
                    return fund;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("fund").Where("id", id).AsDelete();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.RemoveAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.RemoveAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Fund fund)
        {
            var sqlQuery = "UPDATE \"fund\" SET" +
                             "\"name_text\"=@name_text, " +
                             "\"date_time_added\"=@data_create," +
                             "\"id_user\"=@ID_User," +
                             " \"description\"=@description " +
                              "WHERE \"id\"=@id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    LogQuery(sqlQuery, fund.GetType(), fund);
                    await dbConnection.ExecuteAsync(sqlQuery, fund);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.UpdateAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(FundRepository)}.{nameof(FundRepository.UpdateAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }
    }
}
