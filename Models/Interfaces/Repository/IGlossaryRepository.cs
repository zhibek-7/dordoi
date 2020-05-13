using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.PartialEntities.Glossaries;

namespace Models.Interfaces.Repository
{
    public interface IGlossaryRepository : IRepositoryAsync<Glossary>
    {
        Task<Guid?> AddNewTermAsync(Guid glossaryId, TranslationSubstring newTerm, Guid? partOfSpeechId);
        Task DeleteTermAsync(Guid glossaryId, Guid termId);
        Task<IEnumerable<Term>> GetAssotiatedTermsByGlossaryIdAsync(Guid? glossaryId, int limit, int offset, string termPart = null, string[] sortBy = null, bool sortAscending = true);
        Task<int?> GetAssotiatedTermsCountAsync(Guid? glossaryId, string termPart);
        Task<Locale> GetLocaleByIdAsync(Guid glossaryId);
        Task<Locale> GetLocaleByTermByIdAsync(Guid termId);
        Task UpdateTermAsync(Guid glossaryId, TranslationSubstring updatedTerm, Guid? partOfSpeechId);

        Task<IEnumerable<Locale>> GetTranslationLocalesAsync(Guid glossaryId);

        Task<Glossary> GetByFileIdAsync(Guid fileId);

        Task<IEnumerable<TermWithGlossary>> GetAllTermsFromAllGlossarisInProjectByIdAsync(Guid projectId);

        /// <summary>
        /// Удаление всех терминов глоссария
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        Task DeleteTermsByGlossaryAsync(Guid glossaryId);
    }
}
