using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class localization_projects_glossaries
    {
        public Guid id_localization_project { get; set; }
        public Guid id_glossary { get; set; }
    }
}
