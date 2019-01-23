using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Models.DatabaseEntities.PartialEntities.Glossaries;

namespace Models.Services
{
    public class GlossaryService
    {

        private readonly IGlossaryRepository _glossaryRepository;

        private readonly ITranslationSubstringRepository _stringsRepository;

        public GlossaryService(IGlossaryRepository glossaryRepository, ITranslationSubstringRepository translationSubstringRepository)
        {
            this._glossaryRepository = glossaryRepository;
            this._stringsRepository = translationSubstringRepository;
        }

        public async Task<IEnumerable<Locale>> GetTranslationLocalesForTermAsync(int glossaryId, int termId)
        {
            return await this._stringsRepository.GetLocalesForStringAsync(translationSubstringId: termId);
        }

        public async Task<bool> UpdateAsync(Glossary updatedGlossary)
        {
            return await this._glossaryRepository.UpdateAsync(item: updatedGlossary);
        }

        public async Task<Glossary> GetByIDAsync(int glossaryId)
        {
            return await this._glossaryRepository.GetByIDAsync(id: glossaryId);
        }

        public async Task<IEnumerable<Glossary>> GetAllAsync()
        {
            return await this._glossaryRepository.GetAllAsync();
        }

        public async Task<Locale> GetLocaleByIdAsync(int glossaryId)
        {
            return await this._glossaryRepository.GetLocaleByIdAsync(glossaryId: glossaryId);
        }

        public async Task UpdateTranslationLocalesForTermAsync(int glossaryId, int termId, IEnumerable<int> localesIds)
        {
            await this._stringsRepository.DeleteTranslationLocalesAsync(translationSubstringId: termId);
            await this._stringsRepository.AddTranslationLocalesAsync(translationSubstringId: termId, localesIds: localesIds);
        }

        public async Task<IEnumerable<Term>> GetAssotiatedTermsByGlossaryIdAsync(
            int glossaryId,
            int? limit,
            int? offset,
            string termPart = null,
            string[] sortBy = null,
            bool? sortAscending = true)
        {
            return await this._glossaryRepository.GetAssotiatedTermsByGlossaryIdAsync(
                glossaryId: glossaryId,
                termPart: termPart,
                limit: limit ?? 25,
                offset: offset ?? 0,
                sortBy: sortBy,
                sortAscending: sortAscending ?? true
                );
        }

        public async Task AddNewTermAsync(int glossaryId, TranslationSubstring newTerm, int? partOfSpeechId)
        {
            var newTermId = await this._glossaryRepository.AddNewTermAsync(
                glossaryId: glossaryId,
                newTerm: newTerm,
                partOfSpeechId: partOfSpeechId);
            var glossaryLocales = await this._glossaryRepository.GetTranslationLocalesAsync(glossaryId: glossaryId);
            await this._stringsRepository.AddTranslationLocalesAsync(
                translationSubstringId: newTermId,
                localesIds: glossaryLocales.Select(locale => locale.ID));
        }

        public async Task DeleteTermAsync(int glossaryId, int termId)
        {
            await this._glossaryRepository.DeleteTermAsync(glossaryId: glossaryId, termId: termId);
        }

        public async Task<int> GetAssotiatedTermsCountAsync(int glossaryId, string termPart)
        {
            return await this._glossaryRepository.GetAssotiatedTermsCountAsync(glossaryId: glossaryId, termPart: termPart);
        }

        public async Task UpdateTermAsync(int glossaryId, TranslationSubstring updatedTerm, int? partOfSpeechId)
        {
            await this._glossaryRepository.UpdateTermAsync(
                glossaryId: glossaryId,
                updatedTerm: updatedTerm,
                partOfSpeechId: partOfSpeechId);
        }

        public async Task<IEnumerable<TermWithGlossary>> GetAllTermsFromAllGlossarisInProjectByIdAsync(int projectId)
        {
            return await _glossaryRepository.GetAllTermsFromAllGlossarisInProjectByIdAsync(projectId);
        }

        /// <summary>
        /// Удаление всех терминов глоссария
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        public async Task DeleteTermsByGlossaryAsync(int glossaryId)
        {
            await this._glossaryRepository.DeleteTermsByGlossaryAsync(glossaryId);
        }
    }
}
