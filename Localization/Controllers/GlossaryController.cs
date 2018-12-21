using DAL.Reposity.PostgreSqlRepository;
﻿using Microsoft.AspNetCore.Mvc;
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

        public GlossaryController()
        {
            this._glossaryService = new GlossaryService(new GlossaryRepository());
        }

        [HttpGet]
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

        [HttpGet("{glossaryId}")]
        public async Task<Glossary> GetGlossaryByIdAsync(int glossaryId)
        {
            return await this._glossaryService.GetByIDAsync(glossaryId: glossaryId);
        }

        [HttpGet("{glossaryId}/locale")]
        public async Task<Locale> GetGlossaryLocaleAsync(int glossaryId)
        {
            return await this._glossaryService.GetLocaleByIdAsync(glossaryId: glossaryId);
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
                value: (await this._glossaryService.GetAssotiatedTermsCountAsync(
                    glossaryId: glossaryId,
                    termPart: termSearch)).ToString());

            return await this._glossaryService
                .GetAssotiatedTermsByGlossaryIdAsync(
                    glossaryId: glossaryId,
                    termPart: termSearch,
                    limit: limit,
                    offset: offset,
                    sortBy: sortBy,
                    sortAscending: sortAscending);
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

        [HttpGet("{glossaryId}/terms/{termId}/locales")]
        public async Task<IEnumerable<Locale>> GetTranslationLocalesForTermAsync(int glossaryId, int termId)
        {
            return await this._glossaryService.GetActualTranslationLocalesForTermAsync(
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
