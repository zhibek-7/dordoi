namespace Models.DatabaseEntities
{
    /// <summary>
    /// Глоссарий со связанными данными без группировки по объектам.
    /// Для получения результата запроса.
    /// </summary>
    public class Glossaries : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ID_File { get; set; }

        public int? LocaleID { get; set; }
        public string LocaleName { get; set; }

        public int? LocalizationProjectID { get; set; }
        public string LocalizationProjectName { get; set; }
    }
}
