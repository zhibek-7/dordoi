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
        Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(int fileId, int? localeId);

        /// <summary>
        /// Возвращает скриншоты для определенной строки
        /// </summary>
        /// <param name="translationSubstringId">id строки для которой требуются скриншоты</param>
        /// <returns></returns>
        Task<IEnumerable<Image>> GetImagesOfTranslationSubstringAsync(int translationSubstringId);

        /// <summary>
        /// Загружает скриншот к определенной строке
        /// </summary>
        /// <param name="image">Скриншот</param>
        /// <param name="translationSubstringId">id строки к которой загружается скриншот</param>
        /// <returns></returns>
        Task<int> UploadImageAsync(Image image, int translationSubstringId);

        /// <summary>
        /// Возвращает текущий статус строки (устарел, т.к. параметр status в таблице отсутсвует)
        /// </summary>
        /// <param name="translationSubstringId">id строки для которой необходимо получить текущий статус</param>
        /// <param name="localeId">id языка на котором требуется получить текущий статус строки</param>
        /// <returns></returns>
        Task<string> GetStatusOfTranslationSubstringAsync(int translationSubstringId, int? localeId);

        /// <summary>
        /// Устанавливает текущий статус строки (устарел, т.к. параметр status в таблице отсутсвует)
        /// </summary>
        /// <param name="translationSubstringId">id строки для которой необходимо установить текущий статус</param>
        /// <param name="status">id языка на котором требуется установить текущий статус строки</param>
        /// <returns></returns>
        Task SetStatusOfTranslationSubstringAsync(int translationSubstringId, string status);


        Task<IEnumerable<TranslationSubstring>> GetByProjectIdAsync(
            int projectId,
            int offset,
            int limit,
            int? fileId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true);
        Task<int> GetByProjectIdCountAsync(
            int projectId,
            int? fileId = null,
            string searchString = null);

        Task<IEnumerable<Locale>> GetLocalesForStringAsync(int translationSubstringId);
        Task AddTranslationLocalesAsync(int translationSubstringId, IEnumerable<int> localesIds);
        Task DeleteTranslationLocalesAsync(int translationSubstringId);


        /// <summary>
        /// Удаление всех строк связанных с памятью переводов.
        /// </summary>
        /// <param name="translationMemoryId"></param>
        /// <returns></returns>
        Task<bool> RemoveByTranslationMemoryAsync(int translationMemoryId);

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
            int projectId,
            int offset,
            int limit,
            int? translationMemoryId = null,
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
        Task<int> GetAllWithTranslationMemoryByProjectCountAsync(
            int projectId,
            int? translationMemoryId = null,
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
        Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
    }
}
