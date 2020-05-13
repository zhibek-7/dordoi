using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Для редактирования поля translations.translated
    /// </summary>
    [Serializable]
    public class TranslationDTO : BaseEntity
    {
        public string translated { get; set; }

        public string locale_name { get; set; }
    }
}
