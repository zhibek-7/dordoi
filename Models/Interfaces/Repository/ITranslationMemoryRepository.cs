using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ITranslationMemoryRepository : IBaseRepositoryAsync<TranslationMemory>
    {
        /// <summary>
        /// Добавление новой памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Новая память переводов.</param>
        /// <returns></returns>
        Task AddAsync(TranslationMemoryForEditingDTO translationMemory);

        /// <summary>
        /// Возвращает память переводов для редактирования (без группировки по объектам).
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        Task<IEnumerable<TranslationMemory>> GetForEditAsync(int translationMemoryId);

        /// <summary>
        /// Возвращает список памятей переводов назначенных на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns>TranslationMemoryForSelectDTO</returns>
        Task<IEnumerable<TranslationMemoryForSelectDTO>> GetForSelectByProjectAsync(int projectId);

        /// <summary>
        /// Сохранение изменений в памяти переводов.
        /// </summary>
        /// <param name="translationMemory">Отредактированная память переводов.</param>
        /// <returns></returns>
        Task UpdateAsync(TranslationMemoryForEditingDTO translationMemory);

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
    }
}
