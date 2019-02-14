namespace Models.DatabaseEntities.DTO
{
    public class UserPasswordChangeDTO //: BaseEntity
    {
        public string Name_text { get; set; }
        public string PasswordCurrent { get; set; }
        public string PasswordNew { get; set; }
    }
}
