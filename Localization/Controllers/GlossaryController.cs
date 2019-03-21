﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.PartialEntities.Glossaries;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Localization.Controllers;
using Microsoft.AspNetCore.Authorization;
using System;

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

        [Authorize]
        [HttpPost("list")]
        public async Task<IEnumerable<Glossary>> GetAllAsync()
        {
            //TOO поправить потом для фильтрации по проекту
            return await this._glossaryService.GetAllAsync();
        }

        [Authorize]
        [HttpPut("{glossaryId}")]
        public async Task UpdateAsync(Guid glossaryId, [FromBody] Glossary updatedGlossary)
        {
            updatedGlossary.id = glossaryId;
            await this._glossaryService.UpdateAsync(updatedGlossary: updatedGlossary);
        }

        [Authorize]
        [HttpPost("{glossaryId}/get")]
        public async Task<Glossary> GetGlossaryByIdAsync(Guid glossaryId)
        {
            return await this._glossaryService.GetByIDAsync(glossaryId: glossaryId);
        }

        [Authorize]
        [HttpPost("{glossaryId}/locale/get")]
        public async Task<Locale> GetGlossaryLocaleAsync(Guid glossaryId)
        {
            return await this._glossaryService.GetLocaleByIdAsync(glossaryId: glossaryId);
        }

        [Authorize]
        [HttpPost("term/locale/get/{termId}")]
        public async Task<Locale> GetGlossaryLocaleByTermAsync(Guid termId)
        {
            return await this._glossaryService.GetLocaleByTermByIdAsync(termId: termId);
        }


        public class GetAssotiatedTermsParam
        {
            public string termSearch { get; set; }
            public int? limit { get; set; }
            public int? offset { get; set; }
            public string[] sortBy { get; set; }
            public bool? sortAscending { get; set; }
        }

        [Authorize]
        [HttpPost("{glossaryId}/terms/list")]
        public async Task<IEnumerable<Term>> GetAssotiatedTermsAsync(
            Guid glossaryId,
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

        [Authorize]
        [HttpPost("{glossaryId}/terms")]
        public async Task AddTermAsync(Guid glossaryId, [FromBody] TranslationSubstring newTerm, [FromQuery] Guid? partOfSpeechId)
        {
            await this._glossaryService
                .AddNewTermAsync(
                    glossaryId: glossaryId,
                    newTerm: newTerm,
                    partOfSpeechId: partOfSpeechId);
        }

        [Authorize]
        [HttpDelete("{glossaryId}/terms/{termId}")]
        public async Task DeleteTermAsync(Guid glossaryId, Guid termId)
        {
            await this._glossaryService.DeleteTermAsync(glossaryId: glossaryId, termId: termId);
        }

        [Authorize]
        [HttpPut("{glossaryId}/terms/{termId}")]
        public async Task UpdateTermAsync(
            Guid glossaryId,
            Guid termId,
            [FromBody] TranslationSubstring updatedTerm,
            [FromQuery] Guid? partOfSpeechId)
        {
            updatedTerm.id = termId;
            await this._glossaryService
                .UpdateTermAsync(
                    glossaryId: glossaryId,
                    updatedTerm: updatedTerm,
                    partOfSpeechId: partOfSpeechId);
        }

        [Authorize]
        [HttpPost("terms/{termId}/locales/list")]
        public async Task<IEnumerable<Locale>> GetTranslationLocalesForTermAsync(Guid glossaryId, Guid termId)
        {
            return await this._glossaryService.GetTranslationLocalesForTermAsync(
                termId: termId);
        }

        [Authorize]
        [HttpPut("{glossaryId}/terms/{termId}/locales")]
        public async Task SetTranslationLocalesForTermAsync(Guid glossaryId, Guid termId, [FromBody] IEnumerable<Guid> localesIds)
        {
            await this._glossaryService.UpdateTranslationLocalesForTermAsync(
                glossaryId: glossaryId,
                termId: termId,
                localesIds: localesIds);
        }

        /// <summary>
        /// Получить все термины из всех глоссариев присоедененных к проекту локализации, по id необходимого проекта локализации
        /// </summary>
        /// <param name="projectId">id проекта локализации в котором необходимо найти все термины</param>
        /// <returns>Список терминов</returns>
        [Authorize]
        [HttpPost("FindAllTermsInProjects/{projectId}")]
        public async Task<IEnumerable<TermWithGlossary>> GetAllTermsFromAllGlossarisInProjectByIdAsync(Guid projectId)
        {
            var allTerms = await _glossaryService.GetAllTermsFromAllGlossarisInProjectByIdAsync(projectId);

            return allTerms;
        }

    }
}
