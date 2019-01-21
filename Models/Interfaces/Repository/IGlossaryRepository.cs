using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.PartialEntities.Glossary;

namespace Models.Interfaces.Repository
{
    public interface IGlossaryRepository : IRepositoryAsync<Glossary>
    {
        Task<int> AddNewTermAsync(int glossaryId, TranslationSubstring newTerm, int? partOfSpeechId);
        Task DeleteTermAsync(int glossaryId, int termId);
        Task<IEnumerable<Term>> GetAssotiatedTermsByGlossaryIdAsync(int glossaryId, int limit, int offset, string termPart = null, string[] sortBy = null, bool sortAscending = true);
        Task<int> GetAssotiatedTermsCountAsync(int glossaryId, string termPart);
        Task<Locale> GetLocaleByIdAsync(int glossaryId);
        Task UpdateTermAsync(int glossaryId, TranslationSubstring updatedTerm, int? partOfSpeechId);

        Task<IEnumerable<Locale>> GetTranslationLocalesAsync(int glossaryId);

        Task<Glossary> GetByFileIdAsync(int fileId);

        Task<IEnumerable<TermWithGlossary>> GetAllTermsFromAllGlossarisInProjectByIdAsync(int projectId);
    }
}
