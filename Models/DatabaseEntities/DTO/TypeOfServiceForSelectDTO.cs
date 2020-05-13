using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Тип услуги, содержащий только идентификатор и наименование.
    /// Для выборки, например checkbox.
    /// </summary>
    [Serializable]
    public class TypeOfServiceForSelectDTO //: BaseEntity
    {
        public Guid id { get; set; }

        public string name_text { get; set; }
    }
}
