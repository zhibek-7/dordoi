using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Models.DatabaseEntities.PartialEntities.Glossaries;
using System;

namespace Models.Services
{
    public class GlossaryService : BaseService
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
            try
            {
                return await this._stringsRepository.GetLocalesForStringAsync(translationSubstringId: termId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error  \"{termId}\" ", exception), exception);
            }
        }

        public async Task<bool> UpdateAsync(Glossary updatedGlossary)
        {
            try
            {
                return await this._glossaryRepository.UpdateAsync(item: updatedGlossary);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error  \"{updatedGlossary}\" ", exception), exception);
            }
        }

        public async Task<Glossary> GetByIDAsync(int glossaryId)
        {
            try
            {
                return await this._glossaryRepository.GetByIDAsync(id: glossaryId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error  \"{glossaryId}\" ", exception), exception);
            }
        }

        public async Task<IEnumerable<Glossary>> GetAllAsync()
        {
            try
            {
                return await this._glossaryRepository.GetAllAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error", exception), exception);
            }
        }

        public async Task<Locale> GetLocaleByIdAsync(int glossaryId)
        {
            try
            {
                return await this._glossaryRepository.GetLocaleByIdAsync(glossaryId: glossaryId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error  \"{glossaryId}\" ", exception), exception);
            }
        }

        public async Task UpdateTranslationLocalesForTermAsync(int glossaryId, int termId, IEnumerable<int> localesIds)
        {
            try
            {
                await this._stringsRepository.DeleteTranslationLocalesAsync(translationSubstringId: termId);
                await this._stringsRepository.AddTranslationLocalesAsync(translationSubstringId: termId, localesIds: localesIds);
            }
            catch (Exception exception)
            {

                throw new Exception(WriteLn($"Error  \"{glossaryId}\"    \"{termId}\"   \"{localesIds}\" ", exception), exception);
            }
        }

        public async Task<IEnumerable<Term>> GetAssotiatedTermsByGlossaryIdAsync(
            int glossaryId,
            int? limit,
            int? offset,
            string termPart = null,
            string[] sortBy = null,
            bool? sortAscending = true)
        {
            try
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
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error  \"{glossaryId}\" " + exception.Message, exception), exception);
            }
        }

        public async Task AddNewTermAsync(int glossaryId, TranslationSubstring newTerm, int? partOfSpeechId)
        {
            var newTermId = await this._glossaryRepository.AddNewTermAsync(
                glossaryId: glossaryId,
                newTerm: newTerm,
                partOfSpeechId: partOfSpeechId);
            var glossaryLocales = await this._glossaryRepository.GetTranslationLocalesAsync(glossaryId: glossaryId);

            try
            {
                await this._stringsRepository.AddTranslationLocalesAsync(
                    translationSubstringId: newTermId,
                    localesIds: glossaryLocales.Select(locale => locale.id));
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        public async Task DeleteTermAsync(int glossaryId, int termId)
        {
            try
            {
                await this._glossaryRepository.DeleteTermAsync(glossaryId: glossaryId, termId: termId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        public async Task<int> GetAssotiatedTermsCountAsync(int glossaryId, string termPart)
        {
            try
            {
                return await this._glossaryRepository.GetAssotiatedTermsCountAsync(glossaryId: glossaryId, termPart: termPart);

            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        public async Task UpdateTermAsync(int glossaryId, TranslationSubstring updatedTerm, int? partOfSpeechId)
        {
            try
            {
                await this._glossaryRepository.UpdateTermAsync(
                    glossaryId: glossaryId,
                    updatedTerm: updatedTerm,
                    partOfSpeechId: partOfSpeechId);

            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        public async Task<IEnumerable<TermWithGlossary>> GetAllTermsFromAllGlossarisInProjectByIdAsync(int projectId)
        {
            try
            {
                return await _glossaryRepository.GetAllTermsFromAllGlossarisInProjectByIdAsync(projectId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Удаление всех терминов глоссария
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        public async Task DeleteTermsByGlossaryAsync(int glossaryId)
        {
            try
            {
                await this._glossaryRepository.DeleteTermsByGlossaryAsync(glossaryId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }
    }
}
