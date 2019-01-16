namespace Models.DTO.Participants
{
    public class ParticipantDTO : DatabaseEntities.BaseEntity
    {

        public int LocalizationProjectId { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public bool Active { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

    }
}
