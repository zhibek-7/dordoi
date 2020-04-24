using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Dapper;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;
using Npgsql;

namespace Dal.Reposity.PostgreSqlRepository
{
    public class FundRepository : BaseRepository, IRepositoryAuthorizeAsync<Fund>
    {
        public FundRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<Guid?> CreateAsync(FundDTO fund)
        {
            var sqlQuery = "INSERT INTO fund " +
                            "(name_text, description "
                            + ") " +
                            "VALUES('"
                  + fund.fund_text + "','" + fund.fund_description + "'"
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
                 + fund.fund_text + "','" + fund.fund_description + "'" 
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

        public Task<IEnumerable<Fund>> GetAllAsync(Guid? userId, Guid? projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Fund> GetByIDAsync(Guid id, Guid? conditionsId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Fund item)
        {
            throw new NotImplementedException();
        }
    }
}
