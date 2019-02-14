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
        /// <summary>
        /// Имя пользователя (логин/ник)
        /// </summary>
        public string name_text { get; set; }
        
        public Byte[] photo { get; set; }
        
        public string email { get; set; }

        //public bool joined { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string full_name { get; set; }
        
        public int? id_time_zones { get; set; }
        
        public string about_me { get; set; }
        
        public bool? gender { get; set; }

        //Выбранный язык:
        public int? LocaleId { get; set; }
        /// <summary>
        /// Выбранный язык перевода является родным
        /// </summary>
        public bool LocaleIsNative { get; set; }
    }
}
