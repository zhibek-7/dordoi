using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Язык перевода
    /// </summary>
    public class Locale : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Flag { get; set; }
    }
}
