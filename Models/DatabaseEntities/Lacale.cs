using System;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Язык перевода
    /// </summary>
    public class Locale : BaseEntity
    {
        //[Key]
        public int ID { get; set; }

        //[Required]
        public string Name { get; set; }

        //[Required]
        public string description { get; set; }

        //[Required]
        public string flag { get; set; }
    }
}
