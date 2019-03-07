using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Память переводов, содержащая только идентификатор и наименование.
    /// Для выборки, например checkbox.
    /// </summary>
    [Serializable]
    public class TranslationMemoryForSelectDTO : BaseEntity
    {
        public string name_text { get; set; }
    }
}
