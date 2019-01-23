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
                    var sqlQuery = "DELETE * FROM  LocalizationProjects  WHERE ID = '" + id + "'";
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
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sqlQuery = "UPDATE LocalizationProjects SET ID = '" + project.ID + "', Name = '" + project.Name + "', Description = '" + project.Description + "', URL = '" + project.URL + "', Visibility = '" + project.Visibility + "', DateOfCreation = '" + project.DateOfCreation + "', LastActivity = '" + project.LastActivity + "', ID_SourceLocale = '" + project.ID_SourceLocale + "', AbleToDownload = '" + project.AbleToDownload + "', AbleToLeftErrors = '" + project.AbleToLeftErrors + "', DefaultString = '" + project.DefaultString + "', NotifyNew = '" + project.NotifyNew + "', NotifyFinish = '" + project.NotifyFinish + "', NotifyConfirm = '" + project.NotifyConfirm
                        + "', Logo = '" + project.Logo + "'";
                    this.LogQuery(sqlQuery);
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


    }
}
