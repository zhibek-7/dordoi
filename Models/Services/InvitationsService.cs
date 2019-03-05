using System;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Utilities.Mail;

namespace Models.Services
{
    public class InvitationsService : BaseService
    {

        private readonly IInvitationsRepository _invitationsRepository;

        private readonly IMail _mail;

        public InvitationsService(IInvitationsRepository invitationsRepository, IMail mail)
        {
            this._invitationsRepository = invitationsRepository;
            this._mail = mail;
        }

        public async Task AddInvitationAsync(Invitation invitation, string invitationLink)
        {
            await this._invitationsRepository.AddAsync(invitation);
            await this._mail.PostMail(
                msg: $"{invitation.message}{Environment.NewLine}Ссылка для участия в проекте - {invitationLink}",
                subject: "Приглашение в проект перевода",
                emails: new[] { invitation.email }
                );
        }

        public async Task<Invitation> GetInvitationByIdAsync(Guid invitationId)
        {
            return await this._invitationsRepository.GetByIdAsync(invitationId);
        }

    }
}
