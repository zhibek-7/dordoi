using System;

namespace Models.DatabaseEntities
{
    public class Invitation
    {

        public Guid id { get; set; }

        public int id_project { get; set; }

        public int id_role { get; set; }

        public string email { get; set; }

        public string message { get; set; }

    }
}
