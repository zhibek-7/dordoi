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
                    var project = dbConnection.Query<LocalizationProject>(sqlString, new { Id }).FirstOrDefault();

                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                _loggerError.WriteLn("Ошибка в GetByID NpgsqlException ", exception);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                _loggerError.WriteLn("Ошибка в GetByID ", exception);

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
                    IEnumerable<LocalizationProject> users = dbConnection.Query<LocalizationProject>(sqlString);

                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                _loggerError.WriteLn("Ошибка в GetAll NpgsqlException ", exception);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                _loggerError.WriteLn("Ошибка в GetAll ", exception);

                return null;
            }
        }

        public async Task<IEnumerable<LocalizationProjectForSelectDTO>> GetAllForSelectDTOAsync()
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                IEnumerable<LocalizationProjectForSelectDTO> result =
                    await dbConnection.QueryAsync<LocalizationProjectForSelectDTO>
                    ("SELECT \"ID\", \"Name\" FROM \"LocalizationProjects\"");
                return result;
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
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {

                var sqlQuery = "INSERT INTO \"LocalizationProjects\" (\"Name\", \"Description\", \"URL\", \"Visibility\", \"DateOfCreation\", \"LastActivity\", \"ID_SourceLocale\", \"AbleToDownload\", \"AbleToLeftErrors\", \"DefaultString\", \"NotifyNew\", \"NotifyFinish\", \"NotifyConfirm\", \"Logo\") VALUES('"
              + project.Name + "','" + project.Description + "','" + project.URL + "','" + project.Visibility + "','" + project.DateOfCreation + "','"
              + project.LastActivity + "','" + project.ID_SourceLocale + "','" + project.AbleToDownload + "','" + project.AbleToLeftErrors + "','"
              + project.DefaultString + "','" + project.NotifyNew + "','" + project.NotifyFinish + "','" + project.NotifyConfirm + "','" + project.Logo + "')";


                int? projectId = dbConnection.Query<int>(sqlQuery, project).FirstOrDefault();
                project.ID = (int)projectId;

                //Не нужно дважды вызывать
                //db.Execute(sqlQuery, project);
            }
        }

        /// <summary>
        /// Удалить  проект
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProject(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = "DELETE * FROM  LocalizationProjects  WHERE ID = '" + id + "'";
                connection.Execute(sqlQuery, new { id });
            }
        }



        /// <summary>
        /// Обновить проект
        /// </summary>
        /// <param name="project"></param>
        public void UpdateProject(LocalizationProject project)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE LocalizationProjects SET ID = '" + project.ID + "', Name = '" + project.Name + "', Description = '" + project.Description + "', URL = '" + project.URL + "', Visibility = '" + project.Visibility + "', DateOfCreation = '" + project.DateOfCreation + "', LastActivity = '" + project.LastActivity + "', ID_SourceLocale = '" + project.ID_SourceLocale + "', AbleToDownload = '" + project.AbleToDownload + "', AbleToLeftErrors = '" + project.AbleToLeftErrors + "', DefaultString = '" + project.DefaultString + "', NotifyNew = '" + project.NotifyNew + "', NotifyFinish = '" + project.NotifyFinish + "', NotifyConfirm = '" + project.NotifyConfirm
                    + "', Logo = '" + project.Logo + "'";
                dbConnection.Execute(sqlQuery, project);
            }
        }


    }
}
