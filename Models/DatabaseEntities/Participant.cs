using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Participant : BaseEntity
    {

        public Guid? ID_Localization_Project { get; set; }

        public Guid ID_User { get; set; }

        public Guid ID_Role { get; set; }

        public bool Active { get; set; }

    }
}
