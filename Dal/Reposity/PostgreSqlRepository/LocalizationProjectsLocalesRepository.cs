using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    class LocalizationProjectsLocalesRepository : BaseRepository,ILocalizationProjectsLocalesRepository
    {
        public LocalizationProjectsLocalesRepository(string connectionStr) : base(connectionStr)
        {

        }
        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="project"></param>
        public void AddProjectsLocales(LocalizationProjectsLocalesRepository projectLocale)
        {
            var sqlQuery = "INSERT INTO \"LocalizationProjectsLocales\" (\"ID_LocalizationProject\", \"ID_Locale\", \"PercentOfTranslation\", \"PercentOfConfirmed\")" +
                        "VALUES (@ID_LocalizationProject, @ID_Locale, @PercentOfTranslation, @PercentOfConfirmed) ";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQuery, projectLocale);
                    dbConnection.Execute(sqlQuery, projectLocale);
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
        /// Обновление
        /// </summary>
        /// <param name="project"></param>
        public void UpdateProjectsLocales(LocalizationProjectsLocalesRepository projectLocale)
        {
          
            var sqlQuery = "UPDATE \"LocalizationProjectsLocales\" SET" +
                         
                           "\"ID_Locale\"=@ID_Locale," +
                           "\"PercentOfConfirmed\"=@PercentOfConfirmed," +

                           "WHERE \"ID_LocalizationProject\"=@ID_LocalizationProject";




            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQuery, projectLocale);
                    dbConnection.Execute(sqlQuery, projectLocale);
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
