using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface IGlossariesRepository : IBaseRepositoryAsync<Glossaries>
    {
        /// <summary>
        /// Добавление нового глоссария
        /// </summary>
        /// <param name="glossary">Новый глоссарий</param>
        /// <returns></returns>
        Task AddNewGlossaryAsync(GlossariesForEditingDTO glossary);
        /// <summary>
        /// Возвращает глоссарий для редактирования (без группировки по объектам)
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        Task<IEnumerable<Glossaries>> GetGlossaryForEditAsync(int glossaryId);
        /// <summary>
        /// Сохранение изменений в глоссарии
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий</param>
        /// <returns></returns>
        Task EditGlossaryAsync(GlossariesForEditingDTO glossary);
        /// <summary>
        /// Удаление глоссария
        /// </summary>
        /// <param name="id">Идентификатор глоссария</param>
        /// <returns></returns>
        Task DeleteGlossaryAsync(int id);
    }
}
