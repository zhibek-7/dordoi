using Dapper;
using Models.Interfaces.Repository;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Npgsql;
using System;
using Utilities.Data;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class ProjectTranslationMemoryRepository : BaseRepository//, IProjectTranslationMemoryRepository
    {
        public ProjectTranslationMemoryRepository(string connectionStr) : base(connectionStr)
        {
        }


        /// <summary>
        /// Возвращает список связок проект - ((не)назначенных) памяти переводов (без группировки по объектам).
        /// </summary>
        /// <param name="idProject">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectTranslationMemory>> GetByProject(int idProject)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_memories")
                        .LeftJoin("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
                        .LeftJoin(new Query("localization_projects")
                                .Where("localization_projects.id", idProject)
                                .As("projects"),
                            j => j.On("projects.id", "localization_projects_translation_memories.id_localization_project"))
                        .Select(
                            "translation_memories.id as translationMemory_id",
                            "translation_memories.name_text as translationMemory_name_text",
                            "projects.id as project_id",
                            "projects.name_text as project_name_text"
                            );
                    
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var projectTranslationMemory = await dbConnection.QueryAsync<ProjectTranslationMemory>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
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
        public async Task<bool> UpdateProjectTranslationMemories(int idProject, int[] idTranslationMemories, bool isDeleteOldRecords = true)
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
    }
}
