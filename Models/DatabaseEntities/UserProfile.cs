using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Полные данные пользователя со связанными данными без группировки по объектам.
    /// Для получения результата запроса.
    /// </summary>
    [Serializable]
    public class UserProfile : BaseEntity
    {
        public string name_text { get; set; }
        
        public Byte[] photo { get; set; }

        public string email { get; set; }

        //public bool joined { get; set; }

        public string full_name { get; set; }
        
        public int? id_time_zones { get; set; }

        public string about_me { get; set; }

        public bool? gender { get; set; }

        public int? LocaleId { get; set; }
        public bool LocaleIsNative { get; set; }
    }
}
