using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;

namespace DAL.Reposity.PostgreSqlRepository
{
    class UserActionRepository : BaseRepository, IRepositoryAsync<UserAction>
    {
        private PostgreSqlNativeContext context;

        public UserActionRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public async Task<int> AddAsync(UserAction action)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "INSERT INTO \"UserActions\"" +
                               " (\"ID_User\", \"ID_worktype\", \"Description\", \"ID_Locale\", \"ID_File\", \"ID_String\", \"ID_Translation\", \"ID_Project\") " +
                               "VALUES (@iduser, @idworktype, @description, @idlocale, @idfile, @idstring, @idtranslation, @idproject)";
                    var _params = new { action.ID_User, action.ID_worktype,
                                        action.Description, action.ID_Locale,
                                        action.ID_File, action.ID_String,
                                        action.ID_Translation, action.ID_Project };
                    LogQuery(_sql, _params);
                    var insertedId = await dbConnection.ExecuteScalarAsync<int>(_sql, _params);
                    dbConnection.Close();
                    return insertedId;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.AddAsync ", exception);
                return 0;
            }
        }

        public async Task<UserAction> GetByIDAsync(int id)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "SELECT * FROM \"UserActions\" WHERE \"ID\" = @actionId LIMIT 1";
                    var _params = new { id };
                    LogQuery(_sql, _params);
                    var action = await dbConnection.QueryFirstAsync<UserAction>(_sql, _params);
                    dbConnection.Close();
                    return action;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.GetByIDAsync ", exception);
                return null;
            }
        }

        public async Task<IEnumerable<UserAction>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "SELECT * FROM \"UserActions\"";
                    LogQuery(_sql);
                    var types = await dbConnection.QueryAsync<UserAction>(_sql);
                    dbConnection.Close();
                    return types;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.GetAllAsync ", exception);
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
                    var _sql = "DELETE FROM \"UserActions\" WHERE \"ID\" = @WorkTypeId";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);
                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.RemoveAsync ", exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(UserAction action)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "UPDATE \"TypesOfWork\" SET \"Name\"=@name WHERE \"ID\"=@WorkTypeId ";
                    //var _params = new { name = workType.Name, WorkTypeId = workType.ID };
                    //LogQuery(_sql, _params);
                    //await dbConnection.ExecuteAsync(_sql, _params);
                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.UpdateAsync ", exception);
                return false;
            }
        }
    }
}
