﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class WorkTypeRepository : BaseRepository, IRepositoryAsync<WorkType>
    {
        private PostgreSqlNativeContext context;

        public WorkTypeRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public async Task<int> AddAsync(WorkType workType)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "INSERT INTO \"TypesOfWork\" (\"Name\") VALUES (@Name)";
                    var _params = new { workType.Name };
                    LogQuery(_sql, _params);
                    var insertedId = await dbConnection.ExecuteScalarAsync<int>(_sql, _params);
                    dbConnection.Close();
                    return insertedId;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TypeoOfWorkRepository.AddAsync ", exception);
                return 0;
            }
        }

        public async Task<WorkType> GetByIDAsync(int id)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "SELECT * FROM \"TypesOfWork\" WHERE \"ID\" = @WorkTypeId LIMIT 1";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    var workType = await dbConnection.QueryFirstAsync<WorkType>(_sql, _params);
                    dbConnection.Close();
                    return workType;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TypeoOfWorkRepository.GetByIDAsync ", exception);
                return null;
            }
        }

        public async Task<IEnumerable<WorkType>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "SELECT * FROM \"TypesOfWork\"";
                    LogQuery(_sql);
                    var types = await dbConnection.QueryAsync<WorkType>(_sql);
                    dbConnection.Close();
                    return types;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TypeoOfWorkRepository.GetAllAsync ", exception);
                return null;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "DELETE FROM \"TypesOfWork\" WHERE \"ID\" = @WorkTypeId";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);
                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TypeoOfWorkRepository.RemoveAsync ", exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(WorkType workType)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "UPDATE \"TypesOfWork\" SET \"Name\"=@name WHERE \"ID\"=@WorkTypeId ";
                    var _params = new { name = workType.Name, WorkTypeId = workType.ID };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);
                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TypeoOfWorkRepository.UpdateAsync ", exception);
                return false;
            }
        }
    }
}
