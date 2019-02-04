﻿using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Язык перевода
    /// </summary>
    public class Locale : BaseEntity
    {
        [Required]
        public string Name_text { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Flag { get; set; }

        public string url { get; set; }
    }
}
