namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Глоссарий со строками перечислений имен связанных объектов (Locales, LocalizationProjects).
    /// Для отображения, например в таблице.
    /// </summary>
    public class GlossariesTableViewDTO : BaseEntity
    {
        public string Name_text { get; set; }

        public string Locales_Name { get; set; }

        public string Localization_Projects_Name { get; set; }

    }
}
