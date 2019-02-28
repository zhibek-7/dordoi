using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;
using Npgsql;
using Utilities.Logs;
using Models.Interfaces.Repository;
using Models.DatabaseEntities.DTO;
using System.Threading.Tasks;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectRepository : BaseRepository, IRepository<LocalizationProject>, ILocalizationProjectRepository
    {
        /// <summary>
        /// //////мне нужно по ходу переделать запросы под логирования
        /// </summary>
        public LocalizationProjectRepository(string connectionStr) : base(connectionStr)
        {
        }

        // public void Add(LocalizationProject locale)
        // {
        //     throw new NotImplementedException();
        // }

        public LocalizationProject GetByID(int Id)
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM localization_projects WHERE id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id };
                    this.LogQuery(sqlString, param);
                    var project = dbConnection.Query<LocalizationProject>(sqlString, param).FirstOrDefault();
                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetByID)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetByID)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает проект локализации с подробной иформацией из связанных данных.
        /// </summary>
        /// <param name="id">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        public async Task<LocalizationProject> GetWithDetailsById(int id)
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

        public IEnumerable<LocalizationProject> GetAll()
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM localization_projects";

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
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetAll)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetAll)} {nameof(Exception)} ",
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

        // public bool Remove(int Id)
        // {
        //     throw new NotImplementedException();
        // }

        // public void Update(LocalizationProject user)
        // {
        //     throw new NotImplementedException();
        // }




        public async Task<int> AddAsyncInsertProject(LocalizationProject project)
        {
            var sqlQuery = "INSERT INTO localization_projects (name_text, description, url, visibility, date_of_creation, last_activity, id_source_locale, able_to_download, able_to_left_errors, default_string, notify_new, notify_finish, notify_confirm, logo) VALUES('"
                 + project.Name_text + "','" + project.Description + "','" + project.URL + "','" + project.Visibility + "','" + project.Date_Of_Creation + "','"
                 + project.Last_Activity + "','" + project.ID_Source_Locale + "','" + project.Able_To_Download + "','" + project.AbleTo_Left_Errors + "','"
                 + project.Default_String + "','" + project.Notify_New + "','" + project.Notify_Finish + "','" + project.Notify_Confirm + "','" + project.Logo + "')"
                  + "RETURNING localization_projects.id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    this.LogQuery(sqlQuery);
                    int? projectId = await dbConnection.ExecuteScalarAsync<int>(sqlQuery, project);
                    return project.id = (int)projectId;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddAsyncInsertProject)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddAsyncInsertProject)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        /// <summary>
        /// Функция добавления проекта
        /// </summary>
        /// <param name="project"></param>
        public void InsertProject(LocalizationProject project)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sqlQuery = "INSERT INTO localization_projects (name_text, description, url, visibility, date_of_creation, last_activity, id_source_locale, able_to_download, able_to_left_errors, default_string, notify_new, notify_finish, notify_confirm, logo) VALUES('"
                  + project.Name_text + "','" + project.Description + "','" + project.URL + "','" + project.Visibility + "','" + project.Date_Of_Creation + "','"
                  + project.Last_Activity + "','" + project.ID_Source_Locale + "','" + project.Able_To_Download + "','" + project.AbleTo_Left_Errors + "','"
                  + project.Default_String + "','" + project.Notify_New + "','" + project.Notify_Finish + "','" + project.Notify_Confirm + "','" + project.Logo + "')";

                    this.LogQuery(sqlQuery);
                    int? projectId = dbConnection.Query<int>(sqlQuery, project).FirstOrDefault();
                    project.id = (int)projectId;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.InsertProject)} {nameof(NpgsqlException)} ",
                    exception);

            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.InsertProject)} {nameof(Exception)} ",
                    exception);

            }
        }

        /// <summary>
        /// Удалить  проект
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProject(int Id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var sqlQuery = "DELETE FROM  localization_projects  WHERE id= " + Id + "";
                    var param = new { Id };

                    connection.Execute(sqlQuery, param);

                    this.LogQuery(sqlQuery);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.DeleteProject)} {nameof(NpgsqlException)} ",
                    exception);

            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.DeleteProject)} {nameof(Exception)} ",
                    exception);

            }
        }

        /// <summary>
        /// Обновить проект
        /// </summary>
        /// <param name="project"></param>
        public void UpdateProject(LocalizationProject project)
        {
            var sqlQuery = "UPDATE \"localization_projects\" SET" +
                             "\"name_text\"=@Name_text, " +
                             "\"description\"=@Description," +
                             "\"url\"=@URL," +
                             " \"visibility\"=@Visibility," +
                             " \"date_of_creation\"=@Date_Of_Creation," +
                             " \"last_activity\"=@Last_Activity," +
                             " \"id_source_locale\"=@ID_Source_Locale," +

                             " \"able_to_download\"=@Able_To_Download," +
                             " \"able_to_left_errors\"=@AbleTo_Left_Errors," +
                             " \"default_string\"=@Default_String," +
                             " \"notify_new\"=@Notify_New," +
                             " \"notify_finish\"=@Notify_Finish," +
                             " \"notify_confirm\"=@Notify_Confirm," +
                             " \"notify_new_comment\"=@notify_new_comment," +
                             " \"export_only_approved_translations\"=@export_only_approved_translations," +
                             " \"original_if_string_is_not_translated\"=@original_if_string_is_not_translated  " +
                             "WHERE \"id\"=@id";





            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQuery, project.GetType(), project);
                    dbConnection.Execute(sqlQuery, project);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.UpdateProject)} {nameof(NpgsqlException)} ",
                        exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.UpdateProject)} {nameof(Exception)} ",
                    exception);
            }
        }


        // /// <summary>
        // /// Обновить языки в проекте
        // /// </summary>
        // /// <param name="project"></param>
        // public void AddProjectLocales(LocalizationProjectsLocales projectLocales)
        // {
        //     var sqlQuery = "UPDATE localization_projects_locales SET" +
        //                  "percent_of_translation=@PercentOfTranslation," +
        //                  "percent_of_confirmed=@PercentOfConfirmed," +
        //                  "WHERE id_localization_project=@ID_LocalizationProject AND id_locale = @ID_Locale";
        //     try
        //     {
        //         using (var dbConnection = new NpgsqlConnection(connectionString))
        //         {

        //             this.LogQuery(sqlQuery, projectLocales.GetType(), projectLocales);
        //             dbConnection.Execute(sqlQuery, projectLocales);
        //         }
        //     }
        //     catch (NpgsqlException exception)
        //     {
        //         this._loggerError.WriteLn(
        //                 $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddProjectLocales)} {nameof(NpgsqlException)} ",
        //                 exception);
        //     }
        //     catch (Exception exception)
        //     {
        //         this._loggerError.WriteLn(
        //             $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddProjectLocales)} {nameof(Exception)} ",
        //             exception);
        //     }
        // }

        // /// <summary>
        // /// Обновить языки в проекте
        // /// </summary>
        // /// <param name="project"></param>
        // public void DeleteProjectLocales(LocalizationProjectsLocales projectLocales)
        // {
        //     var sqlQuery = "DELETE FROM localization_projects_locales " +
        //                    "WHERE id_localization_project=@ID_LocalizationProject AND id_locale = @ID_Locale";
        //     try
        //     {
        //         using (var dbConnection = new NpgsqlConnection(connectionString))
        //         {

        //             this.LogQuery(sqlQuery, projectLocales.GetType(), projectLocales);
        //             dbConnection.Execute(sqlQuery, projectLocales);
        //         }
        //     }
        //     catch (NpgsqlException exception)
        //     {
        //         this._loggerError.WriteLn(
        //                 $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.DeleteProjectLocales)} {nameof(NpgsqlException)} ",
        //                 exception);
        //     }
        //     catch (Exception exception)
        //     {
        //         this._loggerError.WriteLn(
        //             $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.DeleteProjectLocales)} {nameof(Exception)} ",
        //             exception);
        //     }
        // }

    }
}
