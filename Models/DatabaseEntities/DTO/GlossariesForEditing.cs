using System.Collections.Generic;

namespace Models.DatabaseEntities.DTO
{
    public class GlossariesForEditing : BaseEntity
    {
        // public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ID_File { get; set; }

        public IEnumerable<int?> LocalesIds { get; set; }

        public IEnumerable<int?> LocalizationProjectsIds { get; set; }
    }
}
