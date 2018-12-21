using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.Glossaries;
using Models.Interfaces.Repository;

namespace Models.Services
{
    public class GlossaryService
    {

        private readonly IGlossaryRepository _glossaryRepository;

        public GlossaryService(IGlossaryRepository glossaryRepository)
        {
            this._glossaryRepository = glossaryRepository;
        }

        public async Task<IEnumerable<Locale>> GetActualTranslationLocalesForTermAsync(int glossaryId, int termId)
        {
            var translationLocalesForTerm = await this._glossaryRepository.GetTranslationLocalesForTermAsync(
                glossaryId: glossaryId,
                termId: termId);
            if (!translationLocalesForTerm.Any())
            {
                translationLocalesForTerm = await this._glossaryRepository.GetTranslationLocalesAsync(glossaryId: glossaryId);
            }
            return translationLocalesForTerm;
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
            var newLocalesIds = localesIds.ToHashSet();
            var glossaryTranslationLocalesIds = (await
                this._glossaryRepository.GetTranslationLocalesAsync(glossaryId: glossaryId))
                    .Select(locale => locale.ID)
                    .ToHashSet();
            await this._glossaryRepository.DeleteTranslationLocalesForTermAsync(termId: termId);
            if (newLocalesIds.Count == glossaryTranslationLocalesIds.Count
                && newLocalesIds.All(newLocaleId => glossaryTranslationLocalesIds.Contains(newLocaleId))
                && glossaryTranslationLocalesIds.All(glossaryLocaleId => newLocalesIds.Contains(glossaryLocaleId)))
            {
                return;
            }
            await this._glossaryRepository.AddTranslationLocalesForTermAsync(termId: termId, localesIds: newLocalesIds);
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
            await this._glossaryRepository.AddNewTermAsync(
                glossaryId: glossaryId,
                newTerm: newTerm,
                partOfSpeechId: partOfSpeechId);
        }

        public async Task DeleteTermAsync(int glossaryId, int termId)
        {
            await this._glossaryRepository.DeleteTermAsync(glossaryId: glossaryId, termId: termId);
        }

        public async Task<int> GetAssotiatedTermsCountAsync(int glossaryId, string termPart)
        {
            return await this._glossaryRepository.GetAssotiatedTermsCountAsync(glossaryId: glossaryId, termPart : termPart);
        }

        public async Task UpdateTermAsync(int glossaryId, TranslationSubstring updatedTerm, int? partOfSpeechId)
        {
            await this._glossaryRepository.UpdateTermAsync(
                glossaryId: glossaryId,
                updatedTerm: updatedTerm,
                partOfSpeechId: partOfSpeechId);
        }

    }
}
