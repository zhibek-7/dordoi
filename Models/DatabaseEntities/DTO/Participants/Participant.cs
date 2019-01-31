namespace Models.DatabaseEntities.DTO.Participants
{
    public class ParticipantDTO : DatabaseEntities.BaseEntity
    {

        public int Localization_Project_Id { get; set; }

        public int User_Id { get; set; }

        public int Role_Id { get; set; }

        public bool Active { get; set; }

        public string User_Name { get; set; }

        public string Role_Name { get; set; }

        public string Role_Short { get; set; }
    }
}
