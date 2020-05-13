using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Память переводов со строками перечислений имен связанных объектов (Locales, LocalizationProjects).
    /// Для отображения, например в таблице.
    /// </summary>
    [Serializable]
    public class TranslationMemoryTableViewDTO : BaseEntity
    {
        public string name_text { get; set; }

        public int string_count { get; set; }

        public string locales_name { get; set; }

        public string localization_projects_name { get; set; }
    }
}
