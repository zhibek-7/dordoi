using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class User: BaseEntity
    {
        //[Required]
        public string Name { get; set; }

        //[Required]
        public string Password { get; set; }

        public Byte[] Photo { get; set; }

        //[Required]
        public string Email { get; set; }

        public bool Joined { get; set; }
    }
}
