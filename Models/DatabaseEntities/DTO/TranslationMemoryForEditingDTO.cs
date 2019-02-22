using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Память переводов со списками идентификатор связанных данных (Locales, LocalizationProjects).
    /// Для модификации, например создание, редактирование.
    /// </summary>
    [Serializable]
    public class TranslationMemoryForEditingDTO : BaseEntity
    {
        public string name_text { get; set; }

        public int? id_file { get; set; }

        public IEnumerable<int?> locales_ids { get; set; }

        public IEnumerable<int?> localization_projects_ids { get; set; }
    }
}
