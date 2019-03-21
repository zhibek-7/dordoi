using Dapper;
using Models.Interfaces.Repository;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models.DatabaseEntities;
using Npgsql;
using System;
using System.Data;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class ProjectTranslationMemoryRepository : BaseRepository, IProjectTranslationMemoryRepository
    {
        public ProjectTranslationMemoryRepository(string connectionStr) : base(connectionStr)
        {
        }


        /// <summary>
        /// Возвращает список связок проект - ((не)назначенных) памяти переводов (без группировки по объектам).
        /// </summary>
        /// <param name="idProject">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectTranslationMemory>> GetByProject(Guid idProject)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    string SQLQuery = @"SELECT translation_memories.id AS translationMemory_id, 
	   translation_memories.name_text AS translationMemory_name_text, 
	   localization_projects_translation_memories.id_localization_project AS project_id
FROM translation_memories 
LEFT JOIN localization_projects_translation_memories ON 
		  localization_projects_translation_memories.id_translation_memory = translation_memories.id and 
		  localization_projects_translation_memories.id_localization_project = @idProject";
                    var param = new { idProject };
                    this.LogQuery(SQLQuery, param);
                    var projectTranslationMemory = await dbConnection.QueryAsync<ProjectTranslationMemory>(SQLQuery, param);
                    return projectTranslationMemory;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(ProjectTranslationMemoryRepository)}.{nameof(ProjectTranslationMemoryRepository.GetByProject)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(ProjectTranslationMemoryRepository)}.{nameof(ProjectTranslationMemoryRepository.GetByProject)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Пересоздание в таблице "localization_projects_translation_memories" связей проекта локализации c памятями переводов (localization_projects с translation_memories).
        /// </summary>
        /// <param name="idProject">Идентификатор проекта локализации.</param>
        /// <param name="idTranslationMemories">Идентификаторы памятей переводов.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task<bool> UpdateProjectTranslationMemories(Guid idProject, Guid[] idTranslationMemories, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryDelete = new Query("localization_projects_translation_memories")
                            .Where("id_localization_project", idProject)
                            .AsDelete();
                        var compiledQueryDelete = _compiler.Compile(queryDelete);
                        LogQuery(compiledQueryDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryDelete.Sql,
                            param: compiledQueryDelete.NamedBindings);
                    }

                    var projectTranslationMemoriesforQuery = idTranslationMemories.Select(t => new
                    {
                        id_translation_memory = t,
                        id_localization_project = idProject
                    }).ToList();

                    foreach (var element in projectTranslationMemoriesforQuery)
                    {
                        var queryInsert = new Query("localization_projects_translation_memories")
                            .AsInsert(element);
                        var compiledQueryInsert = _compiler.Compile(queryInsert);
                        LogQuery(compiledQueryInsert);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryInsert.Sql,
                                param: compiledQueryInsert.NamedBindings);
                    }

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(ProjectTranslationMemoryRepository)}.{nameof(ProjectTranslationMemoryRepository.UpdateProjectTranslationMemories)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(ProjectTranslationMemoryRepository)}.{nameof(ProjectTranslationMemoryRepository.UpdateProjectTranslationMemories)} {nameof(Exception)} ", exception);
                return false;
            }
        }



        /*/// <summary>
        /// Пересоздание в таблице "localization_projects_translation_memories" связей памяти переводов с проектами локализации (translation_memories с localization_projects).
        /// Удаляются старые записи, в которых указаны проекты назначенные на пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="localizationProjectsIds">Выбранные проекты локализации.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task<bool> UpdateTranslationMemoriesLocalizationProjectsAsync(Guid userId,
            Guid translationMemoryId, IEnumerable<Guid> localizationProjectsIdsbool, NpgsqlConnection dbConnection, IDbTransaction transaction, bool isDeleteOldRecords = true)

        {
            //using (var dbConnection = new NpgsqlConnection(connectionString))
            //{
            return await UpdateTranslationMemoriesLocalizationProjectsAsync(userId, translationMemoryId,
                localizationProjectsIdsbool, dbConnection, transaction, isDeleteOldRecords);
            //}

        }
        */

        /// <summary>
        /// Пересоздание в таблице "localization_projects_translation_memories" связей памяти переводов с проектами локализации (translation_memories с localization_projects).
        /// Удаляются старые записи, в которых указаны проекты назначенные на пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="localizationProjectsIds">Выбранные проекты локализации.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task<bool> UpdateTranslationMemoriesLocalizationProjectsAsync(Guid userId,
            Guid translationMemoryId, IEnumerable<Guid> localizationProjectsIds, bool isDeleteOldRecords = true)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                return await UpdateTranslationMemoriesLocalizationProjectsAsync(userId, translationMemoryId,
                    localizationProjectsIds, dbConnection, null, isDeleteOldRecords);
            }
        }

        /// <summary>
        /// Пересоздание в таблице "localization_projects_translation_memories" связей памяти переводов с проектами локализации (translation_memories с localization_projects).
        /// Удаляются старые записи, в которых указаны проекты назначенные на пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="localizationProjectsIds">Выбранные проекты локализации.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task<bool> UpdateTranslationMemoriesLocalizationProjectsAsync(Guid userId, Guid translationMemoryId, IEnumerable<Guid> localizationProjectsIds, NpgsqlConnection dbConnection, IDbTransaction transaction, bool isDeleteOldRecords = true)
        {
            try
            {
                //using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryDelete = new Query("localization_projects_translation_memories")
                            .Where("id_translation_memory", translationMemoryId)
                            .WhereIn("id_localization_project",
                                new Query("localization_projects_translation_memories")
                                    .Join("participants", "participants.id_localization_project", "localization_projects_translation_memories.id_localization_project")
                                    .WhereTrue("participants.active")
                                    .Where("participants.id_user", userId)
                                    .Where("localization_projects_translation_memories.id_translation_memory", translationMemoryId)
                                    .Select("localization_projects_translation_memories.id_localization_project")
                                )
                            .AsDelete();
                        var compiledQueryDelete = _compiler.Compile(queryDelete);
                        LogQuery(compiledQueryDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryDelete.Sql,
                            param: compiledQueryDelete.NamedBindings, transaction: transaction);
                    }

                    var localizationProjectsTranslationMemories = localizationProjectsIds.Select(t => new
                    {
                        id_translation_memory = translationMemoryId,
                        id_localization_project = t
                    }).ToList();

                    foreach (var element in localizationProjectsTranslationMemories)
                    {
                        var queryInsert = new Query("localization_projects_translation_memories").AsInsert(element);
                        var compiledQueryInsert = _compiler.Compile(queryInsert);
                        LogQuery(compiledQueryInsert);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryInsert.Sql,
                                param: compiledQueryInsert.NamedBindings, transaction: transaction);
                    }

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(ProjectTranslationMemoryRepository.UpdateTranslationMemoriesLocalizationProjectsAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationMemoryRepository)}.{nameof(ProjectTranslationMemoryRepository.UpdateTranslationMemoriesLocalizationProjectsAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }
    }
}
