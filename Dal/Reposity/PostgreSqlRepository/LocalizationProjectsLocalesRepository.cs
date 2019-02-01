using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectsLocalesRepository : BaseRepository,  ILocalizationProjectsLocalesRepository
    {
        public LocalizationProjectsLocalesRepository(string connectionStr) : base(connectionStr)
        {

        }

        public IEnumerable<LocalizationProjectsLocales> GetAll(int Id)
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM \"LocalizationProjectsLocales\"" +
                 "WHERE \"ID_LocalizationProject\"=@Id";

            try
            {
                var param = new { Id };

                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlString, param);
                    IEnumerable<LocalizationProjectsLocales> projLocales = dbConnection.Query<LocalizationProjectsLocales>(sqlString, param);
                    return projLocales;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.GetAll)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.GetAll)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }




        public IEnumerable<LocalizationProjectsLocales> GetAll()
        {
            throw new NotImplementedException();
        }

        public LocalizationProjectsLocales GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(LocalizationProjectsLocales item)
        {
            throw new NotImplementedException();
        }

        public void Add(LocalizationProjectsLocales locale)
        {
            throw new NotImplementedException();
        }

     


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="project"></param>
        public void AddProjectsLocales(LocalizationProjectsLocales projectLocale)
        {
            var sqlQuery = "INSERT INTO \"LocalizationProjectsLocales\" (\"ID_LocalizationProject\", \"ID_Locale\", \"PercentOfTranslation\", \"PercentOfConfirmed\")" +
                        "VALUES (@ID_LocalizationProject, @ID_Locale, @PercentOfTranslation, @PercentOfConfirmed) "+
                        "RETURNING  \"LocalizationProjectsLocales\".\"ID_LocalizationProject\"";
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
                        $"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.AddProjectsLocales)} {nameof(NpgsqlException)} ",
                        exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.AddProjectsLocales)} {nameof(Exception)} ",
                    exception);
            }
        }

        

        /// <summary>
        /// Обновление
        /// </summary>
        /// <param name="project"></param>
        public void UpdateProjectsLocales(LocalizationProjectsLocales projectLocale)
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
                        $"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.UpdateProjectsLocales)} {nameof(NpgsqlException)} ",
                        exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.UpdateProjectsLocales)} {nameof(Exception)} ",
                    exception);
            }
        }

    }
}
