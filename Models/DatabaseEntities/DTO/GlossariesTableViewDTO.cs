using System;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Глоссарий со строками перечислений имен связанных объектов (Locales, LocalizationProjects).
    /// Для отображения, например в таблице.
    /// </summary>
    [Serializable]
    public class GlossariesTableViewDTO : BaseEntity
    {
        public string name_text { get; set; }

        public string locales_name { get; set; }

        public string localization_projects_name { get; set; }

    }
}
