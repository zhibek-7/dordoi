using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public abstract class BaseEntity
    {
        [Key]
        public int id { get; set; }
    }
}
