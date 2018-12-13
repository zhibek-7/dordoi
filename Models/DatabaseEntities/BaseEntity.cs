using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    public abstract class BaseEntity
    {
        [Key]
        public int ID { get; set; }
    }
}
