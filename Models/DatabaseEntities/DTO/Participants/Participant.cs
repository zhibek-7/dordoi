using System;

namespace Models.DatabaseEntities.DTO.Participants
{
    [Serializable]
    public class ParticipantDTO : DatabaseEntities.BaseEntity
    {

        public Guid Localization_Project_Id { get; set; }

        public Guid User_Id { get; set; }

        public Guid Role_Id { get; set; }

        public bool Active { get; set; }

        public string User_Name { get; set; }

        public string Role_Name { get; set; }

        public string Role_Short { get; set; }
    }
}
