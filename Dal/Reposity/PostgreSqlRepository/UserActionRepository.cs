using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class UserActionRepository : BaseRepository, IRepositoryAsync<UserAction>
    {
        private PostgreSqlNativeContext context;

        public UserActionRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Добавить произвольное действие пользователя
        /// </summary>
        /// <param name="action">Модель действия</param>
        /// <returns>Идентификатор добавленого действия</returns>
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
                    var _params = new
                    {
                        action.ID_User,
                        action.ID_worktype,
                        action.Description,
                        action.ID_Locale,
                        action.ID_File,
                        action.ID_String,
                        action.ID_Translation,
                        action.ID_Project
                    };
                    LogQuery(_sql, _params);
                    var insertedId = await dbConnection.ExecuteScalarAsync<int>(_sql, _params);
                    return insertedId;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.AddAsync ", exception);
                return 0;
            }
        }

        /// <summary>
        /// Получить действие пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дейтсвия</param>
        /// <returns>Действие пользователя</returns>
        public async Task<UserAction> GetByIDAsync(int id)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "SELECT \"UserActions\".*,\"WorkTypes\".\"Name\" as Worktype  FROM \"UserActions\" join \"WorkTypes\" on \"UserActions\".\"ID_worktype\" = \"WorkTypes\".\"ID\" WHERE \"ID\" = @actionId LIMIT 1";
                    var _params = new { id };
                    LogQuery(_sql, _params);
                    var action = await dbConnection.QueryFirstAsync<UserAction>(_sql, _params);
                    return action;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.GetByIDAsync ", exception);
                return null;
            }
        }

        /// <summary>
        /// Получить список действий всех пользователей системы
        /// </summary>
        /// <returns>Список действий</returns>
        public async Task<IEnumerable<UserAction>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "SELECT a.\"ID\", " +
                               "u.\"Name\" as user, " +
                               "w.\"Name\" as worktype, " +
                               "a.\"Datetime\", " +
                               "a.\"Description\", " +
                               "l.\"Name\" as locale, " +
                               "f.\"Name\" as fileName, " +
                               "s.\"Value\" as string, " +
                               "t.\"Translated\" as translation, " +
                               "p.\"Name\" as project " +
                               "FROM public.\"UserActions\" a " +
                               "join public.\"Users\" u on a.\"ID_User\" = u.\"ID\" " +
                               "join public.\"WorkTypes\" w on a.\"ID_worktype\" = w.\"ID\" " +
                               "left join public.\"Locales\" l on a.\"ID_Locale\" = l.\"ID\" " +
                               "left join public.\"Files\" f on a.\"ID_File\" = f.\"ID\" " +
                               "left join public.\"TranslationSubstrings\" s on a.\"ID_String\" = s.\"ID\" " +
                               "left join public.\"Translations\" t on a.\"ID_Translation\"= s.\"ID\" " +
                               "left join public.\"LocalizationProjects\" p on a.\"ID_Project\" = p.\"ID\"";
                    LogQuery(_sql);
                    var actions = await dbConnection.QueryAsync<UserAction>(_sql);
                    return actions;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.GetAllAsync ", exception);
                return null;
            }
        }

        /// <summary>
        /// Получить список действий пользователей на определенно проекте
        /// </summary>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns>Список действий</returns>
        public async Task<IEnumerable<UserAction>> GetAllByProjectIdAsync(int projectId)
        {
            try
            {
                using (var dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var _sql = "SELECT * FROM \"UserActions\" WHERE \"ID_Project\" = @projectId LIMIT 1";
                    var _params = new { projectId };
                    LogQuery(_sql, _params);
                    var actions = await dbConnection.QueryAsync<UserAction>(_sql, _params);
                    return actions;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.GetAllAsync ", exception);
                return null;
            }
        }

        /// <summary>
        /// Удалить действие пользователя системы. Предполагается что использоваться эта функция не должна
        /// </summary>
        /// <param name="id">Идентификатор действия</param>
        /// <returns>True or false</returns>
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
                    return true;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в UserActionRepository.RemoveAsync ", exception);
                return false;
            }
        }

        /// <summary>
        /// Обновить действие пользователя. Производиться не должно. Выбрасывает исключение 
        /// </summary>
        /// <param name="action">Действие пользователя</param>
        /// <returns>True or false</returns>
        public async Task<bool> UpdateAsync(UserAction action)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавить запись об авторизации
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddAuthorizeActionAsync(int userId, string comment = "")
        {
            UserAction act = new UserAction(userId, "Авторизация", (int) WorkTypes.Authorize);
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о входе в систему
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddLoginActionAsync(int userId, string comment = "")
        {
            UserAction act = new UserAction(userId, "Вход в систему. " + comment, (int) WorkTypes.Login);
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о создании проекта
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddCreateProjectActionAsync(int userId, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            UserAction act = new UserAction(userId, "Создание проекта. " + comment, (int) WorkTypes.CreateProject);
            act.ID_Project = projectId;
            act.ID_Locale = localeId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о добавлении файла
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="fileId">Идентификатор файла</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddAddFileActionAsync(int userId, int projectId, int fileId, string comment = "")
        {   
            UserAction act = new UserAction(userId, "Добавлен файл. " + comment, (int) WorkTypes.AddFile);
            act.ID_Project = projectId;
            act.ID_File = fileId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об обновлении файла
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="fileId">Идентификатор файла</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddUpdateFileActionAsync(int userId, int projectId, int fileId, string comment = "")
        {   
            UserAction act = new UserAction(userId, "Обновление файла. " + comment, (int) WorkTypes.UpdateFile);
            act.ID_Project = projectId;
            act.ID_File = fileId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о добавлении строки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="stringId">Идентификатор строки</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddAddStringActionAsync(int userId, int projectId, int stringId, string comment = "")
        {   
            UserAction act = new UserAction(userId, "Добавление строки. " + comment, (int) WorkTypes.AddString);
            act.ID_Project = projectId;
            act.ID_String = stringId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об обновлении строки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="stringId">Идентификатор строки</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddUpdateStringActionAsync(int userId, int projectId, int stringId, string comment = "")
        {   
            UserAction act = new UserAction(userId, "Редактирование строки. " + comment, (int) WorkTypes.UpdateString);
            act.ID_Project = projectId;
            act.ID_String = stringId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об удалении строки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="stringId">Идентификатор строки</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddDeleteStringActionAsync(int userId, int projectId, int stringId, string comment = "")
        {   
            UserAction act = new UserAction(userId, "Удаление строки. " + comment, (int) WorkTypes.DeleteString);
            act.ID_Project = projectId;
            act.ID_String = stringId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о добавлении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddAddTraslationActionAsync(int userId, int projectId, int translationId, int stringId, int localeId, string comment = "")
        { 
            UserAction act = new UserAction(userId, "Добавление перевода. " + comment, (int) WorkTypes.AddTraslation);
            act.ID_Project = projectId;
            act.ID_Translation = translationId;
            act.ID_String = stringId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об удалении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddDeleteTranslationActionAsync(int userId, int projectId, int translationId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            UserAction act = new UserAction(userId, "Удаление перевода. " + comment, (int) WorkTypes.DeleteTranslation);
            act.ID_Project = projectId;
            act.ID_Translation = translationId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об обновлении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddUpdateTranslationActionAsync(int userId, int projectId, int translationId, int stringId, int localeId, string comment = "")
        {  
            UserAction act = new UserAction(userId, "Редактирование перевода. " + comment, (int) WorkTypes.UpdateTranslation);
            act.ID_Project = projectId;
            act.ID_Translation = translationId;
            act.ID_String = stringId;
            act.ID_Locale = localeId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о подтверждении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddConfirmTranslationActionAsync(int userId, int projectId, int translationId, string comment = "")
        {  
            UserAction act = new UserAction(userId, "Утвержденеи перевода. " + comment, (int) WorkTypes.ConfirmTranslation);
            act.ID_Project = projectId;
            act.ID_Translation = translationId;
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о подтверждении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddChoseTranslationActionAsync(int userId, int projectId, int translationId, string comment = "")
        {  
            UserAction act = new UserAction(userId, "Выбор перевода. " + comment, (int) WorkTypes.ChoseTranslation);
            act.ID_Project = projectId;
            act.ID_Translation = translationId;
            return await AddAsync(act);
        }
    }
}
