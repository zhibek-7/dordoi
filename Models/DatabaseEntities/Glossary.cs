using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class Glossary : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ID_File { get; set; }

    }
}
