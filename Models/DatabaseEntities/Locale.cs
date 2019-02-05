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

        public string code { get; set; }

        public DateTime? data_create { get; set; }

        public string url { get; set; }

        public Locale() { }

        public Locale(string name, string description, string flag, DateTime? data_create, string url)
        {
            this.Name = name;
            this.Description = description;
            this.Flag = flag;
            this.data_create = data_create;
            this.url = url;
        }
    }
}
