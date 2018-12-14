using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Проект локализации
    /// </summary>
    public class LocalizationProject : BaseEntity
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public bool Visibility { get; set; }
        [Required]
        public DateTime DateOfCreation { get; set; }
        [Required]
        public DateTime LastActivity { get; set; }
        [Required]
        public int ID_SourceLocale { get; set; }
        [Required]
        public bool AbleToDownload { get; set; }
        [Required]
        public bool AbleToLeftErrors { get; set; }
        [Required]
        public string DefaultString { get; set; }
        [Required]
        public bool NotifyNew { get; set; }
        [Required]
        public bool NotifyFinish { get; set; }
        [Required]
        public bool NotifyConfirm { get; set; }
        [Required]
        public byte[] Logo { get; set; }
    }
}
