using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class WorkType : BaseEntity
    {
        [Required]
        public string Name_text { get; set; }
    }
}
