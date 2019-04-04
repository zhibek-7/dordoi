using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Models.DatabaseEntities;
using Npgsql;
using Utilities.Logs;
using Models.Interfaces.Repository;
using Models.DatabaseEntities.DTO;
using System.Threading.Tasks;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectRepository : BaseRepository, IRepositoryAuthorizeAsync<LocalizationProject>
    {
        /// <summary>
        /// //////мне нужно по ходу переделать запросы под логирования
        /// </summary>
        public LocalizationProjectRepository(string connectionStr) : base(connectionStr)
        {
        }


        public async Task<LocalizationProject> GetByIDAsync(Guid id, Guid? userId)
        {
            // Sql string to select all rows
            var sqlString = @"SELECT lp.*
            FROM localization_projects as lp
            inner join participants as p

            on lp.id = p.id_localization_project
            where active = true and p.id_user = @userId and lp.id = @Id
            order by lp.name_text";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = id, @userId = userId };
                    this.LogQuery(sqlString, param);
                    var project = dbConnection.Query<LocalizationProject>(sqlString, param).FirstOrDefault();
                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает проект локализации с подробной иформацией из связанных данных.
        /// </summary>
        /// <param name="id">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        public async Task<LocalizationProject> GetWithDetailsById(Guid id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects").Where("localization_projects.id", id)
                        .LeftJoin("locales", "locales.id", "localization_projects.id_source_locale")
                        .Select(new Query("localization_projects").Where("localization_projects.id", id)
                                .LeftJoin("participants", "participants.id_localization_project", "localization_projects.id").Where("active", true)
                                .AsCount("participants.id_user"), "count_users_active")
                        .Select("localization_projects.*", "locales.name_text as source_Locale_Name")
                        .Distinct();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var project = await dbConnection.QueryFirstOrDefaultAsync<LocalizationProject>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetWithDetailsById)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetWithDetailsById)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<IEnumerable<LocalizationProject>> GetAllAsync(Guid? userId, Guid? projectId)
        {

            // Sql string to select all rows
            var sqlString = @"SELECT lp.*
            FROM localization_projects as lp
            inner join participants as p

            on lp.id = p.id_localization_project
            where active = true and p.id_user = '" + (Guid)userId + @"'
            order by lp.name_text";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlString);
                    IEnumerable<LocalizationProject> users = dbConnection.Query<LocalizationProject>(sqlString);
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает список проектов локализации, назначенных на пользователя
        /// </summary>
        /// <param name="userName">логин пользователя</param>
        /// <returns>LocalizationProjectForSelectDTO{ID, Name}</returns>
        public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetForSelectByUserAsync(string userName)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //var sqlString = "SELECT id, name_text FROM localization_projects";
                    //this.LogQuery(sqlString);
                    //IEnumerable<LocalizationProjectForSelectDTO> result =
                    //    await dbConnection.QueryAsync<LocalizationProjectForSelectDTO>
                    //    (sqlString);

                    var query = new Query("localization_projects")
                        .LeftJoin("participants", "participants.id_localization_project", "localization_projects.id")
                        .LeftJoin("users", "users.id", "participants.id_user")
                        .WhereTrue("participants.active")
                        .Where("users.name_text", userName)
                        .Select("localization_projects.id", "localization_projects.name_text");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var result = await dbConnection.QueryAsync<LocalizationProjectForSelectDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return result;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetForSelectByUserAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetForSelectByUserAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<Guid?> AddAsync(CreateLocalizationProject project) //(LocalizationProject project)
        {
            //project.Date_Of_Creation = project.Last_Activity = DateTime.Now;

            var sqlQuery = "INSERT INTO localization_projects " +
                           "(name_text, description, url, " +
                           "visibility, " +
                           "date_of_creation, last_activity, " +
                           "id_source_locale, " +
                           "able_to_download, " +
                           "able_to_left_errors, " +
                           "default_string, " +
                           "notify_new, notify_finish, notify_confirm, " +
                           "logo, " +
                           "notify_new_comment, " +
                           "export_only_approved_translations, " + "original_if_string_is_not_translated, " + "able_translators_change_terms_in_glossaries " +
                           ") " +
                           "VALUES('"
                 + project.Name_text + "','" + project.Description + "','" + project.URL + "','" 
                 + project.Visibility + "','" 
                 + /*project.Date_Of_Creation*/ DateTime.Now + "','" + /*project.Last_Activity*/ DateTime.Now + "','" 
                 + project.ID_Source_Locale + "','" 
                 + project.Able_To_Download + "','" 
                 + project.Able_To_Left_Errors + "','"
                 + project.Default_String + "','" 
                 + project.Notify_New + "','" + project.Notify_Finish + "','" + project.Notify_Confirm + "','" 
                 + project.Logo + "','"
                 
                 + project.notify_new_comment + "','"
                 + project.export_only_approved_translations + "','" + project.original_if_string_is_not_translated + "','" + project.able_translators_change_terms_in_glossaries +

                 "')"
                  + " RETURNING localization_projects.id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    this.LogQuery(sqlQuery);
                    Guid? projectId = await dbConnection.ExecuteScalarAsync<Guid>(sqlQuery, project);
                    return (Guid)projectId;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public Task<Guid?> AddAsync(LocalizationProject item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects").Where("id", id).AsDelete();
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
        
        /// <summary>
        /// Обновление данных проекта локализации.
        /// </summary>
        /// <param name="project">Проект локализации.</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(LocalizationProject project)
        {
            project.Last_Activity = DateTime.Now;

            var sqlQuery = "UPDATE \"localization_projects\" SET" +
                             "\"name_text\"=@Name_text, " +
                             "\"description\"=@Description," +
                             "\"url\"=@URL," +
                             " \"visibility\"=@Visibility," +
                             " \"date_of_creation\"=@Date_Of_Creation," +
                             " \"last_activity\"=@Last_Activity," +
                             " \"id_source_locale\"=@ID_Source_Locale," +

                             " \"able_to_download\"=@Able_To_Download," +
                             " \"able_to_left_errors\"=@Able_To_Left_Errors," +
                             " \"default_string\"=@Default_String," +
                             " \"notify_new\"=@Notify_New," +
                             " \"notify_finish\"=@Notify_Finish," +
                             " \"notify_confirm\"=@Notify_Confirm," +
                             " \"notify_new_comment\"=@notify_new_comment," +
                             " \"export_only_approved_translations\"=@export_only_approved_translations," +
                             " \"original_if_string_is_not_translated\"=@original_if_string_is_not_translated,  " +
                             " \"able_translators_change_terms_in_glossaries\"=@able_translators_change_terms_in_glossaries  " +
                             "WHERE \"id\"=@id";
            
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    LogQuery(sqlQuery, project.GetType(), project);
                    await dbConnection.ExecuteAsync(sqlQuery, project);

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.UpdateAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.UpdateAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }
        
    }
}
