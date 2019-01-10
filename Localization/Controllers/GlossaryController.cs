using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossaryController : ControllerBase
    {

        private readonly GlossaryService _glossaryService;

        public GlossaryController(GlossaryService glossaryService)
        {
            this._glossaryService = glossaryService;
        }

        [HttpPost("list")]
        public async Task<IEnumerable<Glossary>> GetAllAsync()
        {
            return await this._glossaryService.GetAllAsync();
        }

        [HttpPut("{glossaryId}")]
        public async Task UpdateAsync(int glossaryId, [FromBody] Glossary updatedGlossary)
        {
            updatedGlossary.ID = glossaryId;
            await this._glossaryService.UpdateAsync(updatedGlossary: updatedGlossary);
        }

        [HttpPost("{glossaryId}/get")]
        public async Task<Glossary> GetGlossaryByIdAsync(int glossaryId)
        {
            return await this._glossaryService.GetByIDAsync(glossaryId: glossaryId);
        }

        [HttpPost("{glossaryId}/locale/get")]
        public async Task<Locale> GetGlossaryLocaleAsync(int glossaryId)
        {
            return await this._glossaryService.GetLocaleByIdAsync(glossaryId: glossaryId);
        }


        public class GetAssotiatedTermsParam
        {
            public string termSearch { get; set; }
            public int? limit { get; set; }
            public int? offset { get; set; }
            public string[] sortBy { get; set; }
            public bool? sortAscending { get; set; }
        }
        [HttpPost("{glossaryId}/terms/list")]
        public async Task<IEnumerable<Term>> GetAssotiatedTermsAsync(
            int glossaryId,
            [FromBody] GetAssotiatedTermsParam param)
        {
            this.Response.Headers.Add(
                key: "totalCount",
                value: (await this._glossaryService.GetAssotiatedTermsCountAsync(
                    glossaryId: glossaryId,
                    termPart: param.termSearch)).ToString());

            return await this._glossaryService
                .GetAssotiatedTermsByGlossaryIdAsync(
                    glossaryId: glossaryId,
                    termPart: param.termSearch,
                    limit: param.limit,
                    offset: param.offset,
                    sortBy: param.sortBy,
                    sortAscending: param.sortAscending);
        }

        [HttpPost("{glossaryId}/terms")]
        public async Task AddTermAsync(int glossaryId, [FromBody] TranslationSubstring newTerm, [FromQuery] int? partOfSpeechId)
        {
            await this._glossaryService
                .AddNewTermAsync(
                    glossaryId: glossaryId,
                    newTerm: newTerm,
                    partOfSpeechId: partOfSpeechId);
        }

        [HttpDelete("{glossaryId}/terms/{termId}")]
        public async Task DeleteTermAsync(int glossaryId, int termId)
        {
            await this._glossaryService.DeleteTermAsync(glossaryId: glossaryId, termId: termId);
        }

        [HttpPut("{glossaryId}/terms/{termId}")]
        public async Task UpdateTermAsync(
            int glossaryId,
            int termId,
            [FromBody] TranslationSubstring updatedTerm,
            [FromQuery] int? partOfSpeechId)
        {
            updatedTerm.ID = termId;
            await this._glossaryService
                .UpdateTermAsync(
                    glossaryId: glossaryId,
                    updatedTerm: updatedTerm,
                    partOfSpeechId: partOfSpeechId);
        }

        [HttpPost("{glossaryId}/terms/{termId}/locales/list")]
        public async Task<IEnumerable<Locale>> GetTranslationLocalesForTermAsync(int glossaryId, int termId)
        {
            return await this._glossaryService.GetTranslationLocalesForTermAsync(
                glossaryId: glossaryId,
                termId: termId);
        }

        [HttpPut("{glossaryId}/terms/{termId}/locales")]
        public async Task SetTranslationLocalesForTermAsync(int glossaryId, int termId, [FromBody] IEnumerable<int> localesIds)
        {
            await this._glossaryService.UpdateTranslationLocalesForTermAsync(
                glossaryId: glossaryId,
                termId: termId,
                localesIds: localesIds);
        }

    }
}
