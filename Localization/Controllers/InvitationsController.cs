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
            //TODO пока выделим в отдельную рассылку
            string hostName = this._settings.GetString("host_email_name");
            string hostProtocol = this._settings.GetString("host_email_protocol");
            hostProtocol = hostProtocol == "" ? "" : (hostProtocol + "://");
            await this._invitationsService.AddInvitationAsync(invitation, $"{hostProtocol}{hostName}/invitation/{invitation.id}");
        }

        [HttpPost("{invitationId}")]
        [Authorize]
        public async Task<Invitation> GetInvitationByIdAsync(Guid invitationId)
        {
            return await this._invitationsService.GetInvitationByIdAsync(invitationId);
        }

        [HttpPost("activate/{invitationId}")]
        [Authorize]
        public async Task ActivateInvitationAsync(Guid invitationId)
        {
            await this._invitationsService.ActivateInvitationAsync(
                invitationId: invitationId,
                currentUserName: this.User.Identity.Name);
        }

    }
}
