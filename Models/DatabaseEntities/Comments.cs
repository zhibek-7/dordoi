using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class Comments : BaseEntity
    {
        public DateTime DateTime { get; set; }

        public int ID_TranslationSubstrings { get; set; }

        public int ID_User { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
