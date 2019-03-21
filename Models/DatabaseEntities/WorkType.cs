using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class WorkType : BaseEntity
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name_text { get; set; }
    }
}
