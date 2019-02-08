using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class User : BaseEntity
    {
        //[Required]
        public string Name_text { get; set; }

        //[Required]
        public string Password_text { get; set; }

        public Byte[] Photo { get; set; }

        //[Required]
        public string Email { get; set; }

        public bool Joined { get; set; }
    }
}
