using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Comments : BaseEntity
    {
        public DateTime DateTime { get; set; }

        public int ID_Translation_Substrings { get; set; }

        public int ID_User { get; set; }

        [Required]
        public string Comment_text { get; set; }
    }
}
