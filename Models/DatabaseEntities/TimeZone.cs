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
        public string name_text { get; set; }
        public string description { get; set; }
        public string code { get; set; }
    }
}
