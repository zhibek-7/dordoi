using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    public class Translation: BaseEntity
    {
        [Key]
        public int ID { get; set; }

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

        public int ID_Locale { get; set; }
    }
}
