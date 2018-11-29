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
            this._glossaryRepository = new GlossaryRepository();
        }

        [HttpGet]
        public IEnumerable<Glossary> GetAll()
        {
            return this._glossaryRepository.GetAll();
        }

        [HttpPut("{id}")]
        public void Update(int id, [FromBody] Glossary updatedGlossary)
        {
            this._glossaryRepository.Update(updatedGlossary);
        }

        [HttpGet("{id}")]
        public Glossary GetGlossaryById(int id)
        {
            return this._glossaryRepository.GetByID(id: id);
        }

        [HttpGet("{id}/get_assotiated_terms")]
        public IEnumerable<Models.DatabaseEntities.String> GetAssotiatedTerms(int id)
        {
            return this._glossaryRepository.GetAssotiatedTermsByGlossaryId(id: id);
        }

        [HttpPost("{id}/add_term")]
        public void AddTerm(int id, [FromBody] Models.DatabaseEntities.String newTerm)
        {
            this._glossaryRepository.AddNewTerm(glossaryId: id, newTerm: newTerm);
        }

    }
}
