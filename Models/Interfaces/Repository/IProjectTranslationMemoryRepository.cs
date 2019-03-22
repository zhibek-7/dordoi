using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IProjectTranslationMemoryRepository
    {
        /// <summary>
        /// Возвращает список связок проект - ((не)назначенных) памяти переводов (без группировки по объектам).
        /// </summary>
        /// <param name="idProject">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        Task<IEnumerable<ProjectTranslationMemory>> GetByProject(Guid idProject);

        /// <summary>
        /// Пересоздание в таблице "localization_projects_translation_memories" связей проекта локализации c памятями переводов (localization_projects с translation_memories).
        /// </summary>
        /// <param name="idProject">Идентификатор проекта локализации.</param>
        /// <param name="idTranslationMemories">Идентификаторы памятей переводов.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        Task<bool> UpdateProjectTranslationMemories(Guid idProject, Guid[] idTranslationMemories, bool isDeleteOldRecords = true);

        /// <summary>
        /// Пересоздание в таблице "localization_projects_translation_memories" связей памяти переводов с проектами локализации (translation_memories с localization_projects).
        /// Удаляются старые записи, в которых указаны проекты назначенные на пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="localizationProjectsIds">Выбранные проекты локализации.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        Task<bool> UpdateTranslationMemoriesLocalizationProjectsAsync(Guid userId, Guid translationMemoryId, IEnumerable<Guid> localizationProjectsIds, bool isDeleteOldRecords = true);
    }
}
