using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Проблемы перевода
    /// </summary>
    public class TranslationTrouble : BaseEntity
    {
        [Required]
        public int ID_Trouble;

        [Required]
        public string Trouble;

        [Required]
        public int ID_Translation;
    }
}
