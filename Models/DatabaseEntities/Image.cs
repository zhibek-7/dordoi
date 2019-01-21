using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class Image : BaseEntity
    {
        [Required]
        public string URL { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateTimeAdded { get; set; }

        public int ID_User { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
