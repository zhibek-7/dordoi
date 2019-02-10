using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Проблемы перевода
    /// </summary>
    [Serializable]
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
