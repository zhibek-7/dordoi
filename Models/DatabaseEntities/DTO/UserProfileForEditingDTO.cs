using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Пользователь со списком идентификаторов связанных данных (Locales).
    /// Для модификации, например создание, редактирование.
    /// </summary>
    [Serializable]
    public class UserProfileForEditingDTO : BaseEntity
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
        
        /// <summary>
        /// Выбранные языки перевода (идентификатор языка перевода, язык перевода является родным)
        /// </summary>
        public IEnumerable<Tuple<int, bool>> locales_id_is_native { get; set; }
    }
}
