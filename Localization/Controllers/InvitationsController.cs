using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.Services;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : BaseController
    {

        private readonly InvitationsService _invitationsService;

        private readonly ISettings _settings;

        public InvitationsController(InvitationsService invitationsService, ISettings settings)
        {
            this._invitationsService = invitationsService;
            this._settings = settings;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task AddInvitationAsync([FromBody] Invitation invitation)
        {
            string hostName = this._settings.GetString("host_name");
            string hostProtocol = this._settings.GetString("host_protocol");
            await this._invitationsService.AddInvitationAsync(invitation, $"{hostProtocol}://{hostName}/invitation/{invitation.id}");
        }

        [HttpPost("{invitationId}")]
        [Authorize]
        public async Task<Invitation> GetInvitationByIdAsync(Guid invitationId)
        {
            return await this._invitationsService.GetInvitationByIdAsync(invitationId);
        }

    }
}
