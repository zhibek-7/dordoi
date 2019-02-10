using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Participant : BaseEntity
    {

        public int ID_Localization_Project { get; set; }

        public int ID_User { get; set; }

        public int ID_Role { get; set; }

        public bool Active { get; set; }

    }
}
