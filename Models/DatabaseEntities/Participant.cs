using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    public class Participant : BaseEntity
    {

        public int ID_LocalizationProject { get; set; }

        public int ID_User { get; set; }

        public int ID_Role { get; set; }

        public bool Active { get; set; }

    }
}
