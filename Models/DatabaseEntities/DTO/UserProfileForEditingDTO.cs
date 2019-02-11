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
        public string name { get; set; }

        //public string Password { get; set; }

        public Byte[] photo { get; set; }

        public string email { get; set; }

        // public bool joined { get; set; }

        public string full_name { get; set; }

        public int? time_zone { get; set; }

        public string about_me { get; set; }

        public bool? gender { get; set; }

        public IEnumerable<int?> locales_ids { get; set; }
        public IEnumerable<Tuple<int, bool>> locales_id_is_native { get; set; }
    }
}
