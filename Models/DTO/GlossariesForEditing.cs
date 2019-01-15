using Models.DatabaseEntities;
using System.Collections.Generic;

namespace Models.DTO
{
    public class GlossariesForEditing : BaseEntity
    {
        // public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID_File { get; set; }

        public IEnumerable<Locale> Locales { get; set; }

        public IEnumerable<localizationProjectForSelectDTO> LocalizationProjects { get; set; }
    }
}
