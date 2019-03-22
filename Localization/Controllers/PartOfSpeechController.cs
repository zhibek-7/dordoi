using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Localization.Controllers;
using Microsoft.AspNetCore.Authorization;
using Utilities;

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

        [Authorize]
        [HttpPost]
        [Route("List/{glossaryId}")]
        public IEnumerable<PartOfSpeech> GetAll(Guid? glossaryId)
        {
            if (glossaryId != null)
                return this._partOfSpeechRepository.GetByGlossaryId(glossaryId: glossaryId.Value);
            else
                return this._partOfSpeechRepository.GetAll();
        }

    }
}
