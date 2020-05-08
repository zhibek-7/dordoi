using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Fund: BaseEntity
    {
        [Required]
        public string name_text { get; set; }

        [Required]
        public string description { get; set; }

        public DateTime? data_create { get; set; }
        public Guid ID_User { get; set; }
    }
}
