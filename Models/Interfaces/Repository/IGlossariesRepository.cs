﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface IGlossariesRepository : IBaseRepositoryAsync<Glossaries>
    {
        /// <summary>
        /// Возвращает список глоссариев, со строками перечислений имен связанных объектов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        Task<IEnumerable<GlossariesTableViewDTO>> GetAllByUserIdAsync(
            int? userId,
            int offset,
            int limit,
            int? projectId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true);

        /// <summary>
        /// Возвращает количество глоссариев.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <returns></returns>
        Task<int> GetAllByUserIdCountAsync(
            int? userId,
            int? projectId = null,
            string searchString = null);

        /// <summary>
        /// Возвращает глоссарий для редактирования (без группировки по объектам).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        Task<IEnumerable<Glossaries>> GetGlossaryForEditAsync(int glossaryId);

        /// <summary>
        /// Добавление нового глоссария.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="glossary">Новый глоссарий.</param>
        /// <returns></returns>
        Task AddNewGlossaryAsync(int userId, GlossariesForEditingDTO glossary);

        /// <summary>
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="glossary">Отредактированный глоссарий.</param>
        /// <returns></returns>
        Task EditGlossaryAsync(int userId, GlossariesForEditingDTO glossary);

        /// <summary>
        /// удаление глоссария.
        /// </summary>
        /// <param name="id">идентификатор глоссария.</param>
        /// <returns></returns>
        Task DeleteGlossaryAsync(int id);
    }
}
