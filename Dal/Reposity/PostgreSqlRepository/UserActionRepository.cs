using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class UserActionRepository : BaseRepository, IUserActionRepository
    {
        public UserActionRepository(string connectionStr) : base(connectionStr)
        {
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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "INSERT INTO user_actions" +
                               " (id_user, id_work_type, description, id_locale, id_file, id_string, id_translation, id_project, datetime) " +
                               "VALUES (@id_user, @id_work_type, @description, @id_locale, @id_file, @id_string, @id_translation, @id_project, @datetime)";
                    var _params = new
                    {
                        action.id_user,
                        action.id_work_type,
                        action.description,
                        action.id_locale,
                        action.id_file,
                        action.id_string,
                        action.id_translation,
                        action.id_project,
                        action.datetime
                    };
                    LogQuery(_sql, _params);
                    var insertedId = await dbConnection.ExecuteScalarAsync<int>(_sql, _params);
                    return insertedId;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "SELECT user_actions.*,work_types.name_text as Worktype  FROM user_actions join work_types on user_actions.id_work_type = work_types.id WHERE id = @id LIMIT 1";
                    var _params = new { id };
                    LogQuery(_sql, _params);
                    var action = await dbConnection.QueryFirstAsync<UserAction>(_sql, _params);
                    return action;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("user_actions").Select();
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var userActions = await dbConnection.QueryAsync<UserAction>(compiledQuery.Sql, compiledQuery.NamedBindings);
                    return userActions;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        private static readonly Dictionary<string, string> UserActionsSortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "id" },
            { "id_user", "id_user" },
            { "user_name", "user_name" },
            { "id_work_type", "id_work_type" },
            { "work_type_name", "work_type_name" },
            { "datetime", "datetime" },
            { "description", "description" },
            { "id_locale", "id_locale" },
            { "locale_name", "locale_name" },
            { "id_file", "id_file" },
            { "file_name", "file_name" },
            { "id_string", "id_string" },
            { "translation_substring_name", "translation_substring_name" },
            { "id_translation", "id_translation" },
            { "translation", "translation" },
            { "id_project", "id_project" },
            { "project_name", "project_name" },
        };
        /// <summary>
        /// Получить список действий пользователей на определенно проекте
        /// </summary>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns>Список действий</returns>
        public async Task<IEnumerable<UserAction>> GetAllByProjectIdAsync(
            int projectId,
            int offset,
            int limit,
            int workTypeId,
            int userId,
            int localeId,
            string[] sortBy,
            bool sortAscending
            )
        {
            if (sortBy == null)
            {
                sortBy = new[] { "id" };
            }

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetAllByProjectIdQuery(
                        projectId: projectId,
                        workTypeId: workTypeId,
                        userId: userId,
                        localeId: localeId
                        );

                    query = this.ApplyPagination(query, offset, limit);

                    query = this.ApplySorting(
                        query: query,
                        columnNamesMappings: UserActionRepository.UserActionsSortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var userActions = await dbConnection.QueryAsync<UserAction>(compiledQuery.Sql, compiledQuery.NamedBindings);
                    return userActions;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetAllByProjectIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetAllByProjectIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<int> GetAllByProjectIdCountAsync(
            int projectId,
            int workTypeId,
            int userId,
            int localeId
            )
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetAllByProjectIdQuery(
                        projectId: projectId,
                        workTypeId: workTypeId,
                        userId: userId,
                        localeId: localeId
                        );

                    query = query.AsCount();

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    return await dbConnection.ExecuteScalarAsync<int>(compiledQuery.Sql, compiledQuery.NamedBindings);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetAllByProjectIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return -1;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.GetAllByProjectIdAsync)} {nameof(Exception)} ",
                    exception);
                return -1;
            }
        }

        private Query GetAllByProjectIdQuery(
            int projectId,
            int workTypeId,
            int userId,
            int localeId
            )
        {
            var query = new Query("user_actions")
                .Where("id_project", projectId)
                .Select();

            if (workTypeId > 0)
            {
                query = query.Where("id_work_type", workTypeId);
            }

            if (userId > 0)
            {
                query = query.Where("id_user", userId);
            }

            if (localeId > 0)
            {
                query = query.Where("id_locale", localeId);
            }

            return query;
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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "DELETE FROM user_actions WHERE id = @WorkTypeId";
                    var _params = new { WorkTypeId = id };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteAsync(_sql, _params);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.RemoveAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserActionRepository)}.{nameof(UserActionRepository.RemoveAsync)} {nameof(Exception)} ",
                    exception);
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
        public async Task<int> AddAuthorizeActionAsync(int userId, string userName, string comment = "")
        {
            var act = new UserAction(userId, userName, "Авторизация", (int)WorkTypes.Authorize, WorkTypes.Authorize.ToString());
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о входе в систему
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddLoginActionAsync(int userId, string userName, string comment = "")
        {
            var act = new UserAction(userId, userName, "Вход в систему. " + comment, (int)WorkTypes.Login, WorkTypes.Login.ToString());
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о создании проекта
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddCreateProjectActionAsync(int userId, string userName, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, userName, "Создание проекта. " + comment, (int)WorkTypes.CreateProject, WorkTypes.CreateProject.ToString())
            {
                id_project = projectId,
                id_locale = localeId
            };
            return await AddAsync(act);
        }




        /// <summary>
        /// Добавить запись о редактировании проекта
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddEditProjectActionAsync(int userId, string userName, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, userName, "Редактирование проекта. " + comment, (int)WorkTypes.EditProject, WorkTypes.EditProject.ToString())
            {
                id_project = projectId,
                id_locale = localeId
            };
            return await AddAsync(act);
        }


        /// <summary>
        /// Добавить запись о добавлении файла
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="fileId">Идентификатор файла</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddAddFileActionAsync(int userId, string userName, int projectId, int fileId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Добавлен файл. " + comment, (int)WorkTypes.AddFile, WorkTypes.AddFile.ToString())
            {
                id_project = projectId,
                id_file = fileId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавление варианта перевода
        /// </summary>
        /// <param name="item"></param>
        public async Task<int> AddAddFileActionAsync(File item, int? idTranslit, WorkTypes wt)
        {
            var action = new UserAction
            {
                datetime = item.date_of_change,
                id_project = item.id_localization_project,
                id_file = idTranslit,
                file_name = item.name_text,
                id_work_type = (int)wt
            };
            return await AddAsync(action);
        }


        /// <summary>
        /// Добавить запись об обновлении файла
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="fileId">Идентификатор файла</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddUpdateFileActionAsync(int userId, string userName, int projectId, int fileId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Обновление файла. " + comment, (int)WorkTypes.UpdateFile, WorkTypes.UpdateFile.ToString())
            {
                id_project = projectId,
                id_file = fileId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о добавлении строки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="stringId">Идентификатор строки</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddAddStringActionAsync(int userId, string userName, int projectId, int stringId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Добавление строки. " + comment, (int)WorkTypes.AddString, WorkTypes.AddString.ToString())
            {
                id_project = projectId,
                id_string = stringId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об обновлении строки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="stringId">Идентификатор строки</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddUpdateStringActionAsync(int userId, string userName, int projectId, int stringId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Редактирование строки. " + comment, (int)WorkTypes.UpdateString, WorkTypes.UpdateString.ToString())
            {
                id_project = projectId,
                id_string = stringId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об удалении строки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="stringId">Идентификатор строки</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddDeleteStringActionAsync(int userId, string userName, int projectId, int stringId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Удаление строки. " + comment, (int)WorkTypes.DeleteString, WorkTypes.DeleteString.ToString())
            {
                id_project = projectId,
                id_string = stringId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о добавлении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddAddTraslationActionAsync(int userId, string userName, int projectId, int translationId, int stringId, int localeId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Добавление перевода. " + comment, (int)WorkTypes.AddTraslation, WorkTypes.AddTraslation.ToString())
            {
                id_project = projectId,
                id_translation = translationId,
                id_string = stringId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавление варианта перевода
        /// </summary>
        /// <param name="item"></param>
        public async Task<int> AddAddTraslationActionAsync(Translation item, int? idTranslit, WorkTypes wt)
        {
            var action = new UserAction
            {
                id_string = item.ID_String,
                id_translation = idTranslit,
                translation = item.Translated,
                id_work_type = (int)wt,
                id_user = item.ID_User,
                datetime = item.DateTime,
                id_locale = item.ID_Locale
            };

            //TODO нехватает данных, нужно вычислять.
            //action.ID_Project = projectId;
            //action.ID_Translation = translationId;
            return await AddAsync(action);
        }

        /// <summary>
        /// Добавить запись об удалении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddDeleteTranslationActionAsync(int userId, string userName, int projectId, int translationId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, userName, "Удаление перевода. " + comment, (int)WorkTypes.DeleteTranslation, WorkTypes.DeleteTranslation.ToString())
            {
                id_project = projectId,
                id_translation = translationId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись об обновлении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddUpdateTranslationActionAsync(int userId, string userName, int projectId, int translationId, int stringId, int localeId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Редактирование перевода. " + comment, (int)WorkTypes.UpdateTranslation, WorkTypes.UpdateTranslation.ToString())
            {
                id_project = projectId,
                id_translation = translationId,
                id_string = stringId,
                id_locale = localeId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о подтверждении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddConfirmTranslationActionAsync(int userId, string userName, int projectId, int translationId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Утвержденеи перевода. " + comment, (int)WorkTypes.ConfirmTranslation, WorkTypes.ConfirmTranslation.ToString())
            {
                id_project = projectId,
                id_translation = translationId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о подтверждении перевода
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="translationId">Идентификатор перевода</param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddChoseTranslationActionAsync(int userId, string userName, int projectId, int translationId, string comment = "")
        {
            var act = new UserAction(userId, userName, "Выбор перевода. " + comment, (int)WorkTypes.ChoseTranslation, WorkTypes.ChoseTranslation.ToString())
            {
                id_project = projectId,
                id_translation = translationId
            };
            return await AddAsync(act);
        }



        /// <summary>
        /// Добавить запись о создании глоссария
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddCreateGlossaryActionAsync(int userId, string userName, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, userName, "Создание глоссария" + comment, (int)WorkTypes.CreateGlossary, WorkTypes.CreateGlossary.ToString())
            {
                id_project = projectId,
                id_locale = localeId
            };
            return await AddAsync(act);
        }
        /// <summary>
        /// Добавить запись о редактировании глоссария
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddEditGlossaryActionAsync(int userId, string userName, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, userName, "Редактирование глоссария" + comment, (int)WorkTypes.EditGlossary, WorkTypes.EditGlossary.ToString())
            {
                id_project = projectId,
                id_locale = localeId
            };
            return await AddAsync(act);
        }


        /// <summary>
        /// Добавить запись о удалении глоссария
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddDeleteGlossaryActionAsync(int userId, string userName, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, userName, "Удаление глоссария" + comment, (int)WorkTypes.DeleteGlossary, WorkTypes.DeleteGlossary.ToString())
            {
                id_project = projectId,
                id_locale = localeId
            };
            return await AddAsync(act);
        }



        /// <summary>
        /// Добавить запись о создании приглашенного переводчика
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> AddOrActivateParticipantAsync(int userId, string userName, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, projectId.ToString(), "Создание приглашенного пользователя. " + comment, (int)WorkTypes.CreateParticipant, WorkTypes.CreateParticipant.ToString())
            {
                id_project = projectId,
                id_locale = userId
            };
            return await AddAsync(act);
        }

        /// <summary>
        /// Добавить запись о создании приглашенного переводчика
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId"></param>
        /// <returns>Идентификатор добавленого действия</returns>
        public async Task<int> DeleteParticipantAsync(int userId, string userName, int projectId, int localeId, string comment = "")
        {// Authorize = 1, //1	Авторизация пользователя    
            var act = new UserAction(userId, projectId.ToString(), "Удаление приглашенного пользователя. " + comment, (int)WorkTypes.DeleteParticipant, WorkTypes.DeleteParticipant.ToString())
            {
                id_project = projectId,
                id_locale = userId
            };
            return await AddAsync(act);
        }

    }
}
