using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    public class Comments: BaseEntity
    {
        public int ID_Translation { get; set; }

        public int ID_User { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
