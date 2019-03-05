using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.Services;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : BaseController
    {

        private readonly InvitationsService _invitationsService;

        public InvitationsController(InvitationsService invitationsService)
        {
            this._invitationsService = invitationsService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task AddInvitationAsync([FromBody] Invitation invitation)
        {
            string hostName = this.HttpContext.Request.Host.Value;
            await this._invitationsService.AddInvitationAsync(invitation, $"https://{hostName}/invitation/{invitation.id}");
        }

        [HttpPost("{invitationId}")]
        [Authorize]
        public async Task<Invitation> GetInvitationByIdAsync(Guid invitationId)
        {
            return await this._invitationsService.GetInvitationByIdAsync(invitationId);
        }

    }
}
