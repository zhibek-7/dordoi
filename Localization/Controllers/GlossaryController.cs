using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<Models.Glossaries.Term> GetAssotiatedTerms(
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

        [HttpGet("{glossaryId}/terms/{termId}/locales")]
        public IEnumerable<Locale> GetTranslationLocalesForTerm(int glossaryId, int termId)
        {
            var translationLocalesForTerm = this._glossaryRepository.GetTranslationLocalesForTerm(glossaryId, termId);
            if (!translationLocalesForTerm.Any())
            {
                translationLocalesForTerm = this._glossaryRepository.GetTranslationLocales(glossaryId: glossaryId);
            }
            return translationLocalesForTerm;
        }

        [HttpPut("{glossaryId}/terms/{termId}/locales")]
        public void SetTranslationLocalesForTerm(int glossaryId, int termId, [FromBody] IEnumerable<int> localesIds)
        {
            var newLocalesIds = localesIds.ToHashSet();
            var glossaryTranslationLocalesIds =
                this._glossaryRepository
                    .GetTranslationLocales(glossaryId: glossaryId)
                    .Select(locale => locale.ID)
                    .ToHashSet();
            this._glossaryRepository.DeleteTranslationLocalesForTerm(termId: termId);
            if (newLocalesIds.Count == glossaryTranslationLocalesIds.Count
                && newLocalesIds.All(newLocaleId => glossaryTranslationLocalesIds.Contains(newLocaleId))
                && glossaryTranslationLocalesIds.All(glossaryLocaleId => newLocalesIds.Contains(glossaryLocaleId)))
            {
                return;
            }
            this._glossaryRepository.SetTranslationLocalesForTerm(termId: termId, localesIds: newLocalesIds);
        }

    }
}
