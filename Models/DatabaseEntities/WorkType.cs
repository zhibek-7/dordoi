using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class WorkType : BaseEntity
    {
        [Required]
        public string Name_text { get; set; }
    }
}
