using System.Collections.Generic;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Глоссарий со списками идентификатор связанных данных (Locales, LocalizationProjects).
    /// Для модификации, например создание, редактирование.
    /// </summary>
    public class GlossariesForEditingDTO : BaseEntity
    {
        public string Name_text { get; set; }
        public string Description { get; set; }
        public int? ID_File { get; set; }

        public IEnumerable<int?> Locales_Ids { get; set; }

        public IEnumerable<int?> Localization_Projects_Ids { get; set; }
    }
}
