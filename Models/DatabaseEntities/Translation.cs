using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Translation : BaseEntity
    {
        [Required]
        public int ID_String { get; set; }

        [Required]
        public string Translated { get; set; }

        [Required]
        public bool Confirmed { get; set; }

        [Required]
        public int ID_User { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int ID_Locale { get; set; }
    }
}
