using System;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Глоссарий со связанными данными без группировки по объектам.
    /// Для получения результата запроса.
    /// </summary>
    [Serializable]
    public class Glossaries : BaseEntity
    {
        public string Name_text { get; set; }
        public string Description { get; set; }
        public Guid? ID_File { get; set; }

        public Guid Locale_ID { get; set; }
        public string Locale_Name { get; set; }

        public Guid? Localization_Project_ID { get; set; }
        public string Localization_Project_Name { get; set; }
    }
}
