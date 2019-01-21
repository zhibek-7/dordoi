using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartOfSpeechController : ControllerBase
    {

        private readonly PartOfSpeechRepository _partOfSpeechRepository;

        public PartOfSpeechController()
        {
            this._partOfSpeechRepository = new PartOfSpeechRepository(Settings.GetStringDB());
        }

        [HttpGet]
        public IEnumerable<PartOfSpeech> GetAll([FromQuery] int? glossaryId)
        {
            if (glossaryId != null)
                return this._partOfSpeechRepository.GetByGlossaryId(glossaryId: glossaryId.Value);
            else
                return this._partOfSpeechRepository.GetAll();
        }

    }
}
