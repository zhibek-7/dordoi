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
        /// <param name="glossary">новый глоссарий</param>
        /// <returns></returns>
        Task AddNewGlossaryAsync(GlossariesForEditing glossary);
        /// <summary>
        /// Возвращает глоссарий для редактирования (без группировки по объектам) //(со связанными объектами)
        /// </summary>
        /// <param name="glossaryId">идентификатор глоссария</param>
        /// <returns></returns>
        Task<GlossariesForEditing> GetGlossaryForEditAsync(int glossaryId);
        /// <summary>
        /// Сохранение изменений в глоссарии
        /// </summary>
        /// <param name="glossary">отредактированный глоссарий</param>
        /// <returns></returns>
        Task EditGlossaryAsync(GlossariesForEditing glossary);
        /// <summary>
        /// Удаление глоссария
        /// </summary>
        /// <param name="id">идентификатор глоссария</param>
        /// <returns></returns>
        Task DeleteGlossaryAsync(int id);
    }
}
