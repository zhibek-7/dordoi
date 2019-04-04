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
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectsLocalesRepository : BaseRepository, ILocalizationProjectsLocalesRepository
    {
        public LocalizationProjectsLocalesRepository(string connectionStr) : base(connectionStr)
        {

        }


        /// <summary>
        /// Удалить языки в проекте
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task DeleteProjectLocalesByIdAsync(Guid projectId)
        {
            var sqlQuery = "DELETE FROM localization_projects_locales " +
                           "WHERE id_localization_project=@Id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = projectId };
                    LogQuery(sqlQuery, param);
                    await dbConnection.ExecuteAsync(sqlQuery, param);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.DeleteProjectLocalesByIdAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.DeleteProjectLocalesByIdAsync)} {nameof(Exception)} ", exception);
            }
        }

        public async Task AddProjectsLocalesAsync(LocalizationProjectsLocales projectLocale)
        {
            var sqlQuery = "INSERT INTO localization_projects_locales (id_localization_project, id_locale, percent_of_translation, percent_of_confirmed)" +
                           "VALUES (@ID_Localization_Project, @ID_Locale, @Percent_Of_Translation, @Percent_Of_Confirmed) ";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    LogQuery(sqlQuery, projectLocale.GetType(), projectLocale);
                    await dbConnection.ExecuteAsync(sqlQuery, projectLocale);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.AddProjectsLocalesAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.AddProjectsLocalesAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Переназначение языков переводов на проект локализации.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        public async Task UpdateProjectLocales(Guid projectId, IEnumerable<LocalizationProjectsLocales> projectLocales)
        {
            await DeleteProjectLocalesByIdAsync(projectId);

            foreach (var projectLocale in projectLocales)
            {
                await AddProjectsLocalesAsync(projectLocale);
            }
        }

        /// <summary>
        /// Назначение языков переводов на проект локализации.
        /// </summary>
        /// <param name="projectLocales"></param>
        /// <returns></returns>
        public async Task CreateProjectLocales(IEnumerable<LocalizationProjectsLocales> projectLocales)
        {
            foreach (var projectLocale in projectLocales)
            {
                await AddProjectsLocalesAsync(projectLocale);
            }
        }

        /// <summary>
        /// Возвращает назначенные языки переводов на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        public async Task<IEnumerable<LocalizationProjectsLocales>> GetAllByProjectId(Guid projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects_locales")
                        .Where("id_localization_project", projectId)
                        .Select("localization_projects_locales.*");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var projectsLocales = await dbConnection.QueryAsync<LocalizationProjectsLocales>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return projectsLocales;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.GetAllByProjectId)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(LocalizationProjectsLocalesRepository)}.{nameof(LocalizationProjectsLocalesRepository.GetAllByProjectId)} {nameof(Exception)} ", exception);
                return null;
            }

        }
    }
}
