using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    public class WorkType: BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
