using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectsLocalesRepository : BaseRepository, ILocalizationProjectsLocalesRepository
    {
        public LocalizationProjectsLocalesRepository(string connectionStr) : base(connectionStr)
        {

        }

        public async Task<IEnumerable<LocalizationProjectsLocales>> GetAll(int Id)
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM localization_projects_locales     " +
                 "WHERE id_localization_project=@Id";

            try
            {
                var param = new { Id };

                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlString, param);


                    IEnumerable<LocalizationProjectsLocales> projLocales = await dbConnection.QueryAsync<LocalizationProjectsLocales>(sqlString, param);
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
            var sqlQuery = "INSERT INTO localization_projects_locales (id_localization_project, id_locale, percent_of_translation, percent_of_confirmed)" +
                          "VALUES (@ID_Localization_Project, @ID_Locale, @Percent_Of_Translation, @Percent_Of_Confirmed) ";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQuery, projectLocale.GetType(), projectLocale);
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

            var sqlQuery = "UPDATE localization_projects_locales SET" +

                           "id_locale=@ID_Locale," +
                           "percent_of_confirmed=@Percent_Of_Confirmed," +
                           "percent_of_translation=@Percent_Of_Translation  " +
                           "WHERE id_localization_project=@ID_Localization_Project";


            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQuery, projectLocale.GetType(), projectLocale);
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



        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="project"></param>
        public void DeleteProjectsLocales(LocalizationProjectsLocales projectLocale)
        {

            var sqlQuery = "DELETE FROM localization_projects_locales " +
                           "WHERE id_localization_project=@ID_Localization_Project AND " +
                           "id_locale=@ID_Locale";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQuery, projectLocale.GetType(), projectLocale);
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
