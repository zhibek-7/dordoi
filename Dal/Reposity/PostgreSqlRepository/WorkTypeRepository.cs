using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public enum WorkTypes
    {
        Authorize = 1, //1	Авторизация пользователя                                                                            
        Login, //2	Выход пользователя                                                                                  
        CreateProject, //3	Создание проекта                                                                                    
        AddFile, //4	Добавление файла                                                                                    
        UpdateFile, //5	Изменение файла                                                                                     
        AddString, //6	Добавлении строки                                                                                   
        UpdateString, //7	Изменение строки                                                                                    
        DeleteString, //8	Удаление строки                                                                                     
        AddTraslation, //9	Добавление перевода                                                                                 
        DeleteTranslation, //10	Удалении перевода                                                                                   
        UpdateTranslation, //11	Изменение перевода                                                                                  
        ConfirmTranslation, //12	Подтверждение перевода                                                                              
        ChoseTranslation //13	Выбор перевода менеджером
    }

    public class WorkTypeRepository : BaseRepository, IRepositoryAsync<WorkType>
    {
        public WorkTypeRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<int> AddAsync(WorkType workType)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "INSERT INTO \"WorkTypes\" (\"Name\") VALUES (@Name)";
                    var _params = new { workType.Name };
                    LogQuery(_sql, _params);
                    var insertedId = await dbConnection.ExecuteScalarAsync<int>(_sql, _params);

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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "SELECT * FROM \"WorkTypes\" WHERE \"ID\" = @WorkTypeId LIMIT 1";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    var workType = await dbConnection.QueryFirstAsync<WorkType>(_sql, _params);
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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "SELECT * FROM \"WorkTypes\"";
                    LogQuery(_sql);
                    var types = await dbConnection.QueryAsync<WorkType>(_sql);

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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "DELETE FROM \"WorkTypes\" WHERE \"ID\" = @WorkTypeId";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);

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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "UPDATE \"WorkTypes\" SET \"Name\"=@name WHERE \"ID\"=@WorkTypeId ";
                    var _params = new { name = workType.Name, WorkTypeId = workType.ID };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);
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
