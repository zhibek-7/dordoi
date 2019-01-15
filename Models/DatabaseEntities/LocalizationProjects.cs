﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Проект локализации
    /// </summary>
    public class LocalizationProject : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string URL { get; set; }

        public bool Visibility { get; set; }
        [Required]
        public DateTime DateOfCreation { get; set; }
        [Required]
        public DateTime LastActivity { get; set; }
        [Required]
        public int ID_SourceLocale { get; set; }

        public bool AbleToDownload { get; set; }

        public bool AbleToLeftErrors { get; set; }

        public string DefaultString { get; set; }

        public bool NotifyNew { get; set; }

        public bool NotifyFinish { get; set; }

        public bool NotifyConfirm { get; set; }

        public byte[] Logo { get; set; }

        public bool notifynewcomment { get; set; }
    }
}
