using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Пользователь со списком идентификаторов связанных данных (Locales).
    /// Для модификации, например создание, редактирование.
    /// </summary>
    public class UserProfileForEditingDTO : BaseEntity
    {
        public string Name { get; set; }
        
        //public string Password { get; set; }

        public Byte[] Photo { get; set; }
        
        public string Email { get; set; }

        //public bool Joined { get; set; }

        public string FullName { get; set; }

        public int? TimeZone { get; set; }

        public string AboutMe { get; set; }

        public bool? Gender { get; set; }

        public IEnumerable<int?> LocalesIds { get; set; }
        public IEnumerable<Tuple<int, bool>> LocalesIdIsNative { get; set; }
    }
}
