using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Язык перевода
    /// </summary>
    [Serializable]
    public class Locale : BaseEntity
    {
        [Required]
        public string name_text { get; set; }

        [Required]
        public string description { get; set; }


        public bool flag { get; set; }

        [Required]
        public string code { get; set; }

        public DateTime? data_create { get; set; }

        public string url { get; set; }
        public bool is_used { get; set; }

        public Locale() { }

        public Locale(string name, string description, bool flag, DateTime? data_create, string url)
        {
            this.name_text = name;
            this.description = description;
            this.flag = flag;
            this.data_create = data_create;
            this.url = url;
        }

        public Locale(string name, string description, bool flag, string code, DateTime? data_create, string url)
        {
            this.name_text = name;
            this.description = description;
            this.code = code;
            this.flag = flag;
            this.data_create = data_create;
            this.url = url;
        }
        public Locale(int id, string name, string description, bool flag, string code, DateTime data_create, string url)
        {
            base.id = id;
            this.name_text = name;
            this.description = description;
            this.code = code;
            this.flag = flag;
            this.data_create = data_create;
            this.url = url;
        }

    }
}
