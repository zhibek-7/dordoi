using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Полные данные пользователя со связанными данными без группировки по объектам.
    /// Для получения результата запроса.
    /// </summary>
    public class UserProfile : BaseEntity
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
        
        public int? LocaleId { get; set; }
        public bool LocaleIsNative { get; set; }
    }
}
