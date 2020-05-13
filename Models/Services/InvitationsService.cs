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

        private readonly IUserRepository _userRepository;

        private readonly IParticipantRepository _participantRepository;

        public InvitationsService(
            IInvitationsRepository invitationsRepository,
            IMail mail,
            IUserRepository userRepository,
            IParticipantRepository participantRepository
            )
        {
            this._invitationsRepository = invitationsRepository;
            this._mail = mail;
            this._userRepository = userRepository;
            this._participantRepository = participantRepository;
        }

        public async Task AddInvitationAsync(Invitation invitation, string invitationLink)
        {
            await this._invitationsRepository.AddAsync(invitation);
            await this._mail.PostMail(
                msg: $"{invitation.message}{Environment.NewLine}Ссылка для участия в проекте - <a href='{invitationLink}'> {invitationLink} </a>",
                subject: "Приглашение в проект перевода",
                emails: new[] { invitation.email }
                );
        }

        public async Task<Invitation> GetInvitationByIdAsync(Guid invitationId)
        {
            return await this._invitationsRepository.GetByIdAsync(invitationId);
        }

        public async Task ActivateInvitationAsync(Guid invitationId, string currentUserName)
        {
            var invitation = await this.GetInvitationByIdAsync(invitationId);
            var currentUserProfile = await this._userRepository.GetProfileAsync(currentUserName);
            if (currentUserProfile.email == invitation.email)
            {
                await this._participantRepository.AddOrActivateParticipant(
                    projectId: invitation.id_project,
                    userId: currentUserProfile.id,
                    roleId: invitation.id_role
                    );
            }
            else
            {
                throw new Exception("Приглашение относится к другому пользователю.");
            }
        }

    }
}
