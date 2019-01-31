namespace Models.DatabaseEntities
{
    /// <summary>
    /// Глоссарий со связанными данными без группировки по объектам.
    /// Для получения результата запроса.
    /// </summary>
    public class Glossaries : BaseEntity
    {
        public string Name_text { get; set; }
        public string Description { get; set; }
        public int? ID_File { get; set; }

        public int? Locale_ID { get; set; }
        public string Locale_Name { get; set; }

        public int? Localization_Project_ID { get; set; }
        public string Localization_Project_Name { get; set; }
    }
}
