using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class WorkType : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
