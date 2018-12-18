using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Participants
{
    public class Participant : DatabaseEntities.BaseEntity
    {

        public int ID_LocalizationProject { get; set; }

        public int ID_User { get; set; }

        public int ID_Role { get; set; }

        public bool Active { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

    }
}
