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

        public void Add(LocalizationProject locale)
        {
            throw new NotImplementedException();
        }

        public LocalizationProject GetByID(int Id)
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM \"LocalizationProjects\" WHERE \"ID\" = @Id";

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
                    var query = new Query("LocalizationProjects").Where("LocalizationProjects.ID", id)
                        .LeftJoin("Locales", "Locales.ID", "LocalizationProjects.ID_SourceLocale")
                        .Select(new Query("LocalizationProjects").Where("LocalizationProjects.ID", id)
                                .LeftJoin("Participants", "Participants.ID_LocalizationProject", "LocalizationProjects.ID").Where("Active", true)
                                .AsCount("Participants.ID_User"), "CountUsersActive")
                        .Select("LocalizationProjects.*", "Locales.Name as SourceLocaleName")
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
            var sqlString = "SELECT * FROM \"LocalizationProjects\"";

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

        public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetAllForSelectDTOAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {


                    var sqlString = "SELECT \"ID\", \"Name\" FROM \"LocalizationProjects\"";
                    this.LogQuery(sqlString);

                    IEnumerable<LocalizationProjectForSelectDTO> result =
                        await dbConnection.QueryAsync<LocalizationProjectForSelectDTO>
                        (sqlString);
                    return result;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetAllForSelectDTOAsync)} {nameof(NpgsqlException)} ",
                        exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.GetAllForSelectDTOAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(LocalizationProject user)
        {
            throw new NotImplementedException();
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
                    var sqlQuery = "INSERT INTO \"LocalizationProjects\" (\"Name\", \"Description\", \"URL\", \"Visibility\", \"DateOfCreation\", \"LastActivity\", \"ID_SourceLocale\", \"AbleToDownload\", \"AbleToLeftErrors\", \"DefaultString\", \"NotifyNew\", \"NotifyFinish\", \"NotifyConfirm\", \"Logo\") VALUES('"
                  + project.Name + "','" + project.Description + "','" + project.URL + "','" + project.Visibility + "','" + project.DateOfCreation + "','"
                  + project.LastActivity + "','" + project.ID_SourceLocale + "','" + project.AbleToDownload + "','" + project.AbleToLeftErrors + "','"
                  + project.DefaultString + "','" + project.NotifyNew + "','" + project.NotifyFinish + "','" + project.NotifyConfirm + "','" + project.Logo + "')";

                    this.LogQuery(sqlQuery);
                    int? projectId = dbConnection.Query<int>(sqlQuery, project).FirstOrDefault();
                    project.ID = (int)projectId;
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
        public void DeleteProject(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var sqlQuery = "DELETE FROM  \"LocalizationProjects\"  WHERE \"ID\"= " + id + "";
                    connection.Execute(sqlQuery, new { id });

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
            var sqlQuery = "UPDATE \"LocalizationProjects\" SET" +
                           "\"Name\"=@Name, " +
                           "\"Description\"=@Description," +
                           "\"URL\"=@URL," +
                           " \"Visibility\"=@Visibility," +
                           " \"DateOfCreation\"=@DateOfCreation," +
                           " \"LastActivity\"=@LastActivity," +
                           " \"ID_SourceLocale\"=@ID_SourceLocale," +
                           " \"AbleToDownload\"=@AbleToDownload," +
                           " \"AbleToLeftErrors\"=@AbleToLeftErrors," +
                           " \"DefaultString\"=@DefaultString," +
                           " \"NotifyNew\"=@NotifyNew," +
                           " \"NotifyFinish\"=@NotifyFinish," +
                           " \"NotifyConfirm\"=@NotifyConfirm," +
                           " \"notifynewcomment\"=@notifynewcomment," +
                           " \"export_only_approved_translations\"=@export_only_approved_translations," +
                           " \"original_if_string_is_not_translated\"=@original_if_string_is_not_translated  " +
                           "WHERE \"ID\"=@ID";


         
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQuery, project);
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


        /// <summary>
        /// Обновить языки в проекте
        /// </summary>
        /// <param name="project"></param>
        public void AddProjectLocales(LocalizationProjectsLocales projectLocales)
        {
            var sqlQuery = "UPDATE \"LocalizationProjectsLocales\" SET" +
                        "\"PercentOfTranslation\"=@PercentOfTranslation," +
                        "\"PercentOfConfirmed\"=@PercentOfConfirmed," +
                        "WHERE \"ID_LocalizationProject\"=@ID_LocalizationProject AND \"ID_Locale\" = @ID_Locale";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                   
                    this.LogQuery(sqlQuery, projectLocales);
                    dbConnection.Execute(sqlQuery, projectLocales);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddProjectLocales)} {nameof(NpgsqlException)} ",
                        exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.AddProjectLocales)} {nameof(Exception)} ",
                    exception);
            }
        }

        /// <summary>
        /// Обновить языки в проекте
        /// </summary>
        /// <param name="project"></param>
        public void DeleteProjectLocales(LocalizationProjectsLocales projectLocales)
        {
            var sqlQuery = "DELETE FROM \"LocalizationProjectsLocales\" " +
                           "WHERE \"ID_LocalizationProject\"=@ID_LocalizationProject AND \"ID_Locale\" = @ID_Locale";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    this.LogQuery(sqlQuery, projectLocales);
                    dbConnection.Execute(sqlQuery, projectLocales);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                        $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.DeleteProjectLocales)} {nameof(NpgsqlException)} ",
                        exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectRepository)}.{nameof(LocalizationProjectRepository.DeleteProjectLocales)} {nameof(Exception)} ",
                    exception);
            }
        }
    }
}
