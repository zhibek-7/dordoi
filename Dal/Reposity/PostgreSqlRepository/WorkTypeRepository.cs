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
                    var _sql = "INSERT INTO work_types (name_text) VALUES (@Name_text)";
                    var _params = new { workType.Name_text };
                    LogQuery(_sql, _params);
                    var insertedId = await dbConnection.ExecuteScalarAsync<int>(_sql, _params);

                    return insertedId;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        public async Task<WorkType> GetByIDAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "SELECT * FROM work_types WHERE id = @WorkTypeId LIMIT 1";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    var workType = await dbConnection.QueryFirstAsync<WorkType>(_sql, _params);
                    return workType;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<IEnumerable<WorkType>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "SELECT * FROM work_types";
                    LogQuery(_sql);
                    var types = await dbConnection.QueryAsync<WorkType>(_sql);

                    return types;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "DELETE FROM work_types WHERE id = @WorkTypeId";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.RemoveAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.RemoveAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(WorkType workType)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "UPDATE work_types SET name_text=@name WHERE id=@WorkTypeId ";
                    var _params = new { name = workType.Name_text, WorkTypeId = workType.id };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.UpdateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(WorkTypeRepository)}.{nameof(WorkTypeRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }
    }
}
