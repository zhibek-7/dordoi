using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class Glossary : BaseEntity
    {

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ID_File { get; set; }

    }
}
