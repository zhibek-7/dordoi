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
        public Guid ID_Trouble;

        [Required]
        public string Trouble;

        [Required]
        public Guid ID_Translation;
    }
}
