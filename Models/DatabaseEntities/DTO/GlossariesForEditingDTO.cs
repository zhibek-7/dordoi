using System.Collections.Generic;

namespace Models.DatabaseEntities.DTO
{
    //Glossaries with Ids Locales and LocalizationProjects
    public class GlossariesForEditingDTO : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ID_File { get; set; }

        public IEnumerable<int?> LocalesIds { get; set; }

        public IEnumerable<int?> LocalizationProjectsIds { get; set; }
    }
}
