using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Role : BaseEntity
    {

        public string Name_text { get; set; }

        public string Description { get; set; }

    }
}
