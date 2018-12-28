using System.Collections.Generic;
using Models.DatabaseEntities;

namespace Models.DTO
{
    public class Glossaries : BaseEntity
    {
        public string Name { get; set; }
        //public string Description { get; set; }
        //public int ID_File { get; set; }

        public string LocaleID { get; set; }
        public string LocaleName { get; set; }
        
        public string LocalizationProjectID { get; set; }
        public string LocalizationProjectName { get; set; }

        //public Dictionary<int, string> Locales { get; set; }
        //public Dictionary<int, string> LocalizationProjects { get; set; }
    }
}
