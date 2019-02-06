using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public abstract class BaseEntity
    {
        [Key]
        public int id { get; set; }
    }
}
