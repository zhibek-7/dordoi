using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ITranslationSubstringRepository : IRepositoryAsync<TranslationSubstring>
    {
        Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(int fileId, int? localeId);
        //Task<IEnumerable<TranslationSubstring>> GetStringsInVisibleAndCurrentProjectdAsync(int projectId);
        //Task<IEnumerable<TranslationSubstring>> FilterByString(
        //    string filtredString, IEnumerable<TranslationSubstring> filtredListOfStrings);
        Task<IEnumerable<Image>> GetImagesOfTranslationSubstringAsync(int translationSubstringId);
        Task<int> UploadImageAsync(Image image, int translationSubstringId);
        Task<string> GetStatusOfTranslationSubstringAsync(int translationSubstringId);

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
        /// <returns></returns>
        Task<IEnumerable<TranslationSubstringTableViewDTO>> GetAllWithTranslationMemoryByProjectAsync(int projectId);

        Task<IEnumerable<TranslationSubstringTableViewDTO>> GetAllWithTranslationMemoryByProjectAsync(
            int projectId,
            int offset,
            int limit,
            int? translationMemoryId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true);

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
