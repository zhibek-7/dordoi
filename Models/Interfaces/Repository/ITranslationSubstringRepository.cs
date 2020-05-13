using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ITranslationSubstringRepository : IRepositoryAsync<TranslationSubstring>
    {
        //Task<IEnumerable<TranslationSubstring>> GetStringsInVisibleAndCurrentProjectdAsync(int projectId);
        //Task<IEnumerable<TranslationSubstring>> FilterByString(
        //    string filtredString, IEnumerable<TranslationSubstring> filtredListOfStrings);

        /// <summary>
        /// Возвращает все строки принадлежащие определенному файлу вместе с всеми переводами для них (также можно получить переводы только для определенного языка)
        /// </summary>
        /// <param name="fileId">id файла в котором требуется найти все строки</param>
        /// <param name="localeId">(опциональный параметр) id языка на котором нужны варианты перевода</param>
        /// <returns></returns>
        Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(Guid fileId, Guid? localeId);

        /// <summary>
        /// Возвращает скриншоты для определенной строки
        /// </summary>
        /// <param name="translationSubstringId">id строки для которой требуются скриншоты</param>
        /// <returns></returns>
        Task<IEnumerable<Image>> GetImagesOfTranslationSubstringAsync(Guid translationSubstringId);

        /// <summary>
        /// Загружает скриншот к определенной строке
        /// </summary>
        /// <param name="image">Скриншот</param>
        /// <param name="translationSubstringId">id строки к которой загружается скриншот</param>
        /// <returns></returns>
        Task<Guid?> UploadImageAsync(Image image, Guid translationSubstringId);

        /// <summary>
        /// Возвращает текущий статус строки (устарел, т.к. параметр status в таблице отсутсвует)
        /// </summary>
        /// <param name="translationSubstringId">id строки для которой необходимо получить текущий статус</param>
        /// <param name="localeId">id языка на котором требуется получить текущий статус строки</param>
        /// <returns></returns>
        Task<string> GetStatusOfTranslationSubstringAsync(Guid translationSubstringId, Guid? localeId);

        /// <summary>
        /// Устанавливает текущий статус строки (устарел, т.к. параметр status в таблице отсутсвует)
        /// </summary>
        /// <param name="translationSubstringId">id строки для которой необходимо установить текущий статус</param>
        /// <param name="status">id языка на котором требуется установить текущий статус строки</param>
        /// <returns></returns>
        Task SetStatusOfTranslationSubstringAsync(Guid translationSubstringId, string status);


        Task<IEnumerable<TranslationSubstring>> GetByProjectIdAsync(
            Guid projectId,
            int offset,
            int limit,
            Guid? fileId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true);

        Task<int?> GetByProjectIdCountAsync(
            Guid projectId,
            Guid? fileId = null,
            string searchString = null);

        Task<IEnumerable<Locale>> GetLocalesForStringAsync(Guid? translationSubstringId);
        Task AddTranslationLocalesAsync(Guid translationSubstringId, IEnumerable<Guid> localesIds);
        Task DeleteTranslationLocalesAsync(Guid translationSubstringId);


        /// <summary>
        /// Удаление всех строк связанных с памятью переводов.
        /// </summary>
        /// <param name="translationMemoryId"></param>
        /// <returns></returns>
        Task<bool> RemoveByTranslationMemoryAsync(Guid translationMemoryId);

        /// <summary>
        /// Возвращает строки (со связанными объектами).
        /// </summary>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="searchString">Шаблон строки (поиск по substring_to_translate).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        Task<IEnumerable<TranslationSubstringTableViewDTO>> GetAllWithTranslationMemoryByProjectAsync(
            Guid projectId,
            int offset,
            int limit,
            Guid? translationMemoryId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true);

        /// <summary>
        /// Возвращает количество строк.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="searchString">Шаблон строки (поиск по substring_to_translate).</param>
        /// <returns></returns>
        Task<int?> GetAllWithTranslationMemoryByProjectCountAsync(
            Guid projectId,
            Guid? translationMemoryId = null,
            string searchString = null);

        /// <summary>
        /// Обновление поля substring_to_translate
        /// </summary>
        /// <param name="translationSubstring"></param>
        /// <returns></returns>
        Task<bool> UpdateSubstringToTranslateAsync(TranslationSubstringTableViewDTO translationSubstring);

        /// <summary>
        /// Удаление строк.
        /// </summary>
        /// <param name="ids">Идентификаторы строк.</param>
        /// <returns></returns>
        Task<bool> DeleteRangeAsync(IEnumerable<Guid> ids);
    }
}
