using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class Image: BaseEntity
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string URL { get; set; }

        public string Name { get; set; }

        public DateTime DateTimeAdded { get; set; }

        [Required]
        public int ID_User { get; set; }
    }
}
