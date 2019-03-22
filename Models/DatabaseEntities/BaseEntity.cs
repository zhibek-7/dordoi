using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public abstract class BaseEntity
    {
        [Key]
        public Guid id { get; set; }
    }
}
