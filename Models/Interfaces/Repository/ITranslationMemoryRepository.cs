using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ITranslationMemoryRepository : IBaseRepositoryAsync<TranslationMemory>
    {
        /// <summary>
        /// Возвращает памяти переводов (со связанными объектами).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        Task<IEnumerable<TranslationMemoryTableViewDTO>> GetAllByUserIdAsync(
            Guid? userId,
            int offset,
            int limit,
            Guid? projectId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true);

        /// <summary>
        /// Возвращает количество памятей переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
        /// <returns></returns>
        Task<int?> GetAllByUserIdCountAsync(
            Guid? userId,
            Guid? projectId = null,
            string searchString = null);

        /// <summary>
        /// Возвращает память переводов для редактирования (без группировки по объектам).
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        Task<IEnumerable<TranslationMemory>> GetForEditAsync(Guid translationMemoryId);

        /// <summary>
        /// Возвращает список памятей переводов назначенных на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns>TranslationMemoryForSelectDTO</returns>
        Task<IEnumerable<TranslationMemoryForSelectDTO>> GetForSelectByProjectAsync(Guid projectId);

        /// <summary>
        /// Добавление новой памяти переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemory">Новая память переводов.</param>
        /// <returns></returns>
        Task AddAsync(Guid userId, TranslationMemoryForEditingDTO translationMemory);

        /// <summary>
        /// Сохранение изменений в памяти переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemory">Отредактированная память переводов.</param>
        /// <returns></returns>
        Task UpdateAsync(Guid userId, TranslationMemoryForEditingDTO translationMemory);

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
