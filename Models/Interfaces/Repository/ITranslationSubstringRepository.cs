using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface ITranslationSubstringRepository : IRepositoryAsync<TranslationSubstring>
    {
        Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(int fileId);
        Task<IEnumerable<TranslationSubstring>> GetStringsInVisibleAndCurrentProjectdAsync(int projectId);
        Task<IEnumerable<TranslationSubstring>> FilterByString(
            string filtredString, IEnumerable<TranslationSubstring> filtredListOfStrings);
        Task<IEnumerable<Image>> GetImagesOfTranslationSubstringAsync(int translationSubstringId);

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
    }
}
