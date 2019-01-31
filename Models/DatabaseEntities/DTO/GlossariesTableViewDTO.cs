namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Глоссарий со строками перечислений имен связанных объектов (Locales, LocalizationProjects).
    /// Для отображения, например в таблице.
    /// </summary>
    public class GlossariesTableViewDTO : BaseEntity
    {
        public string Name { get; set; }

        public string LocalesName { get; set; }

        public string LocalizationProjectsName { get; set; }

    }
}
