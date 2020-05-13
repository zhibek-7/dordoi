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
    public class IndividualRepository : BaseRepository, IRepositoryAuthorizeAsync<Individual>
    {
        public IndividualRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<Guid?> CreateAsync(IndividualDTO individ)
        {
            var sqlQuery = "INSERT INTO individual " +
                          "(name_text_first, description_first, name_text_second, description_second, name_text_third, description_third, name_text_fourth, description_fourth, name_text_fifth, description_fifth, date_time_added, id_user  "
                          + ") " +
                          "VALUES('"
                + individ.name_text_first + "','" + individ.description_first + "'"
                + individ.name_text_second + "','" + individ.description_second + "'"
                + individ.name_text_third + "','" + individ.description_third + "'"
                + individ.name_text_fourth + "','" + individ.description_fourth + "'"
                + individ.name_text_fifth + "','" + individ.description_fifth + "'"

                 + individ.data_create + "','" + individ.ID_User + "'"
                + ")"
                 + " RETURNING individual.id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    this.LogQuery(sqlQuery);
                    Guid? individId = await dbConnection.ExecuteScalarAsync<Guid>(sqlQuery, individ);
                    return (Guid)individId;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.CreateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.CreateAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

        public async Task<Guid?> AddNewIndividAsync(IndividualDTO individ)
        {
                 var sqlQuery = "INSERT INTO individual " +
                           "(name_text_first, description_first, name_text_second, description_second, name_text_third, description_third, name_text_fourth, description_fourth, name_text_fifth, description_fifth, date_time_added, id_user  "
                           + ") " +
                           "VALUES('"
                 + individ.name_text_first + "','" + individ.description_first + "'"
                 + individ.name_text_second + "','" + individ.description_second + "'"
                 + individ.name_text_third + "','" + individ.description_third + "'"
                 + individ.name_text_fourth + "','" + individ.description_fourth + "'"
                 + individ.name_text_fifth + "','" + individ.description_fifth + "'"

                  + individ.data_create + "','" + individ.ID_User + "'"
                 +  ")"
                  + " RETURNING individual.id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    this.LogQuery(sqlQuery);
                    Guid? individId = await dbConnection.ExecuteScalarAsync<Guid>(sqlQuery, individ);
                    return (Guid)individId;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.AddNewIndividAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.AddNewIndividAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

     

        public async Task<IEnumerable<Individual>> GetAllAsync(Guid? userId, Guid? individId)
        {
            // Sql string to select all rows where id_user = '" + (Guid)userId + @"'
            var sqlString = @"SELECT id, name_text_first, description_first, name_text_second, description_second, name_text_third, description_third, name_text_fourth, description_fourth, name_text_fifth, description_fifth, date_time_added, id_user 
            FROM individual 
            
            order by name_text_first";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlString);
                    IEnumerable<Individual> users = dbConnection.Query<Individual>(sqlString);
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<Individual> GetByIDAsync(Guid id, Guid? userId)
        {
            var sqlString = @"SELECT id, name_text_first, description_first, name_text_second, description_second, name_text_third, description_third, name_text_fourth, description_fourth, name_text_fifth, description_fifth, date_time_added, id_user 
            FROM individual 
            where id_user = '" + (Guid)userId + @"'
            order by name_text_first";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = id, @userId = userId };
                    this.LogQuery(sqlString, param);
                    var individ = dbConnection.Query<Individual>(sqlString, param).FirstOrDefault();
                    return individ;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.GetByIDAsync)} {nameof(Exception)} ",
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
                    var query = new Query("individ").Where("id", id).AsDelete();
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

        public async Task<bool> UpdateAsync(Individual individ)
        {
            var sqlQuery = "UPDATE \"individual\" SET" +
                             "\"name_text_first\"=@name_text_first, " +
                              "\"description_first\"=@description_first, " +

                              "\"name_text_second\"=@name_text_second, " +
                              "\"description_second\"=@description_second, " +

                               "\"name_text_third\"=@name_text_third, " +
                               "\"description_third\"=@description_third, " +

                                "\"name_text_fourth\"=@name_text_fourth, " +
                                "\"description_fourth\"=@description_fourth, " +

                                 "\"name_text_fifth\"=@name_text_fifth, " +
                                 "\"description_fifth\"=@description_fifth, " +

                             "\"date_time_added\"=@data_create," +
                             "\"id_user\"=@ID_User," +
                            
                              "WHERE \"id\"=@id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    LogQuery(sqlQuery, individ.GetType(), individ);
                    await dbConnection.ExecuteAsync(sqlQuery, individ);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.UpdateAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(IndividualRepository)}.{nameof(IndividualRepository.UpdateAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }

        public Task<Guid?> AddAsync(Individual item)
        {
            throw new NotImplementedException();
        }

      

        Task<Individual> IRepositoryAuthorizeAsync<Individual>.GetByIDAsync(Guid id, Guid? conditionsId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Individual>> IRepositoryAuthorizeAsync<Individual>.GetAllAsync(Guid? userId, Guid? projectId)
        {
            throw new NotImplementedException();
        }
    }
}
