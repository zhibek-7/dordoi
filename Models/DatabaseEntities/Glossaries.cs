using System.Collections.Generic;
using Models.DatabaseEntities;
using Models.DTO;

namespace Models.DatabaseEntities
{
    public class Glossaries : BaseEntity
    {
        public string Name { get; set; }

        public IEnumerable<Locale> Locales { get; set; }
        public string LocaleID { get; set; }
        public string LocaleName { get; set; }

        public IEnumerable<localizationProjectForSelectDTO> LocalizationProjects { get; set; }
        public string LocalizationProjectID { get; set; }
        public string LocalizationProjectName { get; set; }
    }
}
