using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Participants
{
    public class Participant : DatabaseEntities.BaseEntity
    {

        public int LocalizationProjectId { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public bool Active { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

    }
}
