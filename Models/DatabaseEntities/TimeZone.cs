using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Часовые пояса
    /// </summary>
    public class TimeZone : BaseEntity
    {
        /// <summary>
        /// Имя часового пояса
        /// </summary>
        public string name_text { get; set; }
        /// <summary>
        /// Отображаемое название часового пояса пользователю
        /// </summary>
        public string description { get; set; }

        public string code { get; set; }
    }
}
