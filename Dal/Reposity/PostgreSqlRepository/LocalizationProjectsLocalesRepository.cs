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
    public class LocalizationProjectsLocalesRepository : BaseRepository, IRepository<LocalizationProjectsLocales>, ILocalizationProjectsLocalesRepository
    {
        public LocalizationProjectsLocalesRepository(string connectionStr) : base(connectionStr)
        {

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
            var sqlQuery = "INSERT INTO localization_projects_ocales (id_localization_project, id_locale, percent_of_translation, percent_of_confirmed)" +
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

            var sqlQuery = "UPDATE localization_projects_ocales SET" +

                           "id_locale=@ID_Locale," +
                           "percent_of_confirmed=@PercentOfConfirmed," +

                           "WHERE id_localization_project=@ID_LocalizationProject";




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
