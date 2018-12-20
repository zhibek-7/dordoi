using DAL.Reposity.Interfaces;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossaryController : ControllerBase
    {

        private readonly IGlossaryRepository _glossaryRepository;

        public GlossaryController()
        {
            this._glossaryRepository = new GlossaryRepository(stringsRepository: new TranslationSubstringRepository());
        }

        [HttpGet]
        public async Task<IEnumerable<Glossary>> GetAllAsync()
        {
            return await this._glossaryRepository.GetAllAsync();
        }

        [HttpPut("{glossaryId}")]
        public async Task Update(int glossaryId, [FromBody] Glossary updatedGlossary)
        {
            updatedGlossary.ID = glossaryId;
            await this._glossaryRepository.UpdateAsync(updatedGlossary);
        }

        [HttpGet("{glossaryId}")]
        public async Task<Glossary> GetGlossaryById(int glossaryId)
        {
            return await this._glossaryRepository.GetByIDAsync(id: glossaryId);
        }

        [HttpGet("{glossaryId}/locale")]
        public async Task<Locale> GetGlossaryLocaleAsync(int glossaryId)
        {
            return await this._glossaryRepository.GetLocaleByIdAsync(glossaryId: glossaryId);
        }

        [HttpGet("{glossaryId}/terms")]
        public async Task<IEnumerable<Models.Glossaries.Term>> GetAssotiatedTermsAsync(
            int glossaryId,
            [FromQuery] string termSearch,
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            [FromQuery] string[] sortBy,
            [FromQuery] bool? sortAscending)
        {
            this.Response.Headers.Add(
                key: "totalCount",
                value: (await this._glossaryRepository.GetAssotiatedTermsCountAsync(
                    glossaryId: glossaryId,
                    termPart: termSearch)).ToString());
            return await this._glossaryRepository
                .GetAssotiatedTermsByGlossaryIdAsync(
                    glossaryId: glossaryId,
                    termPart: termSearch,
                    limit: limit ?? 25,
                    offset: offset ?? 0,
                    sortBy: sortBy,
                    sortAscending: sortAscending ?? true);
        }

        [HttpPost("{glossaryId}/terms")]
        public async Task AddTermAsync(int glossaryId, [FromBody] TranslationSubstring newTerm, [FromQuery] int? partOfSpeechId)
        {
            await this._glossaryRepository
                .AddNewTermAsync(
                    glossaryId: glossaryId,
                    newTerm: newTerm,
                    partOfSpeechId: partOfSpeechId);
        }

        [HttpDelete("{glossaryId}/terms/{termId}")]
        public async Task DeleteTermAsync(int glossaryId, int termId)
        {
            await this._glossaryRepository.DeleteTermAsync(glossaryId: glossaryId, termId: termId);
        }

        [HttpPut("{glossaryId}/terms/{termId}")]
        public async Task UpdateTermAsync(int glossaryId, int termId, [FromBody] TranslationSubstring updatedTerm, [FromQuery] int? partOfSpeechId)
        {
            updatedTerm.ID = termId;
            await this._glossaryRepository
                .UpdateTermAsync(
                    glossaryId: glossaryId,
                    updatedTerm: updatedTerm,
                    partOfSpeechId: partOfSpeechId);
        }

        [HttpGet("{glossaryId}/terms/{termId}/locales")]
        public async Task<IEnumerable<Locale>> GetTranslationLocalesForTermAsync(int glossaryId, int termId)
        {
            return await this._glossaryRepository.GetActualTranslationLocalesForTermAsync(glossaryId, termId);
        }

        [HttpPut("{glossaryId}/terms/{termId}/locales")]
        public async Task SetTranslationLocalesForTermAsync(int glossaryId, int termId, [FromBody] IEnumerable<int> localesIds)
        {
            await this._glossaryRepository.UpdateTranslationLocalesForTermAsync(
                glossaryId: glossaryId,
                termId: termId,
                localesIds: localesIds);
        }

    }
}
