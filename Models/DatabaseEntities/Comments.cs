using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Comments : BaseEntity
    {
        public Guid? ID_User { get; set; }
        public Guid ID_Translation_Substrings { get; set; }
        public string Comment_text { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
