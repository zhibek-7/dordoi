using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectRepository: IRepository<LocalizationProject>
    {
        private PostgreSqlNativeContext context;

        public LocalizationProjectRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
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
                // Using new posgresql connection
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var project = dbConnection.Query<LocalizationProject>(sqlString, new {Id}).FirstOrDefault();
                    dbConnection.Close();
                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

                return null;
            }
        }

        public IEnumerable<LocalizationProject> GetAll()
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM \"LocalizationProjects\"";

            try
            {
                // Using new posgresql connection
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<LocalizationProject> users = dbConnection.Query<LocalizationProject>(sqlString);
                    dbConnection.Close();
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

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
            using (IDbConnection db = context.Connection)
            {

                var sqlQuery = "INSERT INTO LocalizationProjects (Name,Description,URL,Visibility,DateOfCreation,LastActivity,ID_SourceLocale,AbleToDownload,AbleToLeftErrors,DefaultString,NotifyNew,NotifyFinish,NotifyConfirm,Logo) VALUES('"
              + project.Name + "','" + project.Description + "','" + project.URL + "','" + project.Visibility + "','" + project.DateOfCreation + "','"
              + project.LastActivity + "','" + project.ID_SourceLocale + "','" + project.AbleToDownload + "','" + project.AbleToLeftErrors + "','"
              + project.DefaultString + "','" + project.NotifyNew + "','" + project.NotifyFinish + "','" + project.NotifyConfirm + "','" + project.Logo + "')";


                int? projectId = db.Query<int>(sqlQuery, project).FirstOrDefault();
                project.ID = (int)projectId;
                db.Execute(sqlQuery, project);


            }
        }

        /// <summary>
        /// Удалить  проект
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProject(int id)
        {
            using (IDbConnection db = context.Connection)
            {

                var sqlQuery = "DELETE * FROM  LocalizationProjects  WHERE ID = '" + id + "'";
                db.Execute(sqlQuery, new { id });


            }
        }



        /// <summary>
        /// Обновить проект
        /// </summary>
        /// <param name="project"></param>
        public void UpdateProject(LocalizationProject project)
        {
            using (IDbConnection db = context.Connection)
            {
                var sqlQuery = "UPDATE LocalizationProjects SET ID = '" + project.ID + "', Name = '" + project.Name + "', Description = '" + project.Description + "', URL = '" + project.URL + "', Visibility = '" + project.Visibility + "', DateOfCreation = '" + project.DateOfCreation + "', LastActivity = '" + project.LastActivity + "', ID_SourceLocale = '" + project.ID_SourceLocale + "', AbleToDownload = '" + project.AbleToDownload + "', AbleToLeftErrors = '" + project.AbleToLeftErrors + "', DefaultString = '" + project.DefaultString + "', NotifyNew = '" + project.NotifyNew + "', NotifyFinish = '" + project.NotifyFinish + "', NotifyConfirm = '" + project.NotifyConfirm
                    + "', Logo = '" + project.Logo + "'";
                db.Execute(sqlQuery, project);
            }
        }


    }
}
