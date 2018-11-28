using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class Image: BaseEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateTimeAdded { get; set; }

        public int ID_User { get; set; }
    }
}
