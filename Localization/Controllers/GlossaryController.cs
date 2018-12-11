﻿using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using System.Collections.Generic;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossaryController : ControllerBase
    {

        private readonly GlossaryRepository _glossaryRepository;

        public GlossaryController()
        {
            this._glossaryRepository = new GlossaryRepository(stringsRepository: new StringRepository());
        }

        [HttpGet]
        public IEnumerable<Glossary> GetAll()
        {
            return this._glossaryRepository.GetAll();
        }

        [HttpPut("{glossaryId}")]
        public void Update(int glossaryId, [FromBody] Glossary updatedGlossary)
        {
            updatedGlossary.ID = glossaryId;
            this._glossaryRepository.Update(updatedGlossary);
        }

        [HttpGet("{glossaryId}")]
        public Glossary GetGlossaryById(int glossaryId)
        {
            return this._glossaryRepository.GetByID(id: glossaryId);
        }

        [HttpGet("{glossaryId}/locale")]
        public Locale GetGlossaryLocale(int glossaryId)
        {
            return this._glossaryRepository.GetLocaleById(glossaryId: glossaryId);
        }

        [HttpGet("{glossaryId}/terms")]
        public IEnumerable<Models.DatabaseEntities.String> GetAssotiatedTerms(
            int glossaryId,
            [FromQuery] string termSearch,
            [FromQuery] int? pageSize,
            [FromQuery] int? pageNumber,
            [FromQuery] string[] sortBy,
            [FromQuery] bool? sortAscending)
        {
            this.Response.Headers.Add(
                key: "totalCount",
                value: this._glossaryRepository.GetAssotiatedTermsCount(
                    glossaryId: glossaryId,
                    termPart: termSearch).ToString());
            return this._glossaryRepository
                .GetAssotiatedTermsByGlossaryId(
                    glossaryId: glossaryId,
                    termPart: termSearch,
                    pageSize: pageSize ?? 25,
                    pageNumber: pageNumber ?? 1,
                    sortBy: sortBy,
                    sortAscending: sortAscending ?? true);
        }

        [HttpPost("{glossaryId}/terms")]
        public void AddTerm(int glossaryId, [FromBody] Models.DatabaseEntities.String newTerm, [FromQuery] int? partOfSpeechId)
        {
            this._glossaryRepository
                .AddNewTerm(
                    glossaryId: glossaryId,
                    newTerm: newTerm,
                    partOfSpeechId: partOfSpeechId);
        }

        [HttpGet("{glossaryId}/terms/{termId}/part_of_speech")]
        public int? GetTermPartOfSpeech(int glossaryId, int termId)
        {
            return this._glossaryRepository.GetTermPartOfSpeechId(glossaryId: glossaryId, termId: termId);
        }

        [HttpDelete("{glossaryId}/terms/{termId}")]
        public void DeleteTerm(int glossaryId, int termId)
        {
            this._glossaryRepository.DeleteTerm(glossaryId: glossaryId, termId: termId);
        }

        [HttpPut("{glossaryId}/terms/{termId}")]
        public void UpdateTerm(int glossaryId, int termId, [FromBody] Models.DatabaseEntities.String updatedTerm, [FromQuery] int? partOfSpeechId)
        {
            updatedTerm.ID = termId;
            this._glossaryRepository
                .UpdateTerm(
                    glossaryId: glossaryId,
                    updatedTerm: updatedTerm,
                    partOfSpeechId: partOfSpeechId);
        }

    }
}
