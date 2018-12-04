using DAL.Reposity.PostgreSqlRepository;
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

        [HttpGet("{glossaryId}/terms")]
        public IEnumerable<Models.DatabaseEntities.String> GetAssotiatedTerms(int glossaryId, [FromQuery] string termSearch)
        {
            if (string.IsNullOrEmpty(termSearch))
                return this._glossaryRepository.GetAssotiatedTermsByGlossaryId(id: glossaryId);
            else
                return this._glossaryRepository.FindAssotiatedTermsByPart(glossaryId: glossaryId, termPart: termSearch);
        }

        [HttpPost("{glossaryId}/terms")]
        public void AddTerm(int glossaryId, [FromBody] Models.DatabaseEntities.String newTerm)
        {
            this._glossaryRepository.AddNewTerm(glossaryId: glossaryId, newTerm: newTerm);
        }

        [HttpDelete("{glossaryId}/terms/{termId}")]
        public void DeleteTerm(int glossaryId, int termId)
        {
            this._glossaryRepository.DeleteTerm(glossaryId: glossaryId, termId: termId);
        }

        [HttpPut("{glossaryId}/terms/{termId}")]
        public void UpdateTerm(int glossaryId, int termId, [FromBody] Models.DatabaseEntities.String updatedTerm)
        {
            updatedTerm.ID = termId;
            this._glossaryRepository.UpdateTerm(updatedTerm: updatedTerm);
        }

    }
}
