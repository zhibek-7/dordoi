using System;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Тематика, содержащая только идентификатор и наименование.
    /// Для выборки, например checkbox.
    /// </summary>
    [Serializable]
    public class TranslationTopicForSelectDTO //: BaseEntity
    {
        public Guid id { get; set; }

        public string name_text { get; set; }
    }
}
