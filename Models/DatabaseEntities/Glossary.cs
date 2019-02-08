
using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Glossary : BaseEntity
    {
        public string Name_text { get; set; }

        public string Description { get; set; }

        public int ID_File { get; set; }

    }
}
