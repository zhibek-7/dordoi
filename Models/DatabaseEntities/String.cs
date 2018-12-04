using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class String: BaseEntity
    {
        [Key]
        public int ID { get; set; }

        public string SubstringToTranslate { get; set; }

        public string Description { get; set; }

        public string Context { get; set; }

        public int? TranslationMaxLength { get; set; }

        public int ID_FileOwner { get; set; }

        public string Value { get; set; }

        public int? PositionInFullText { get; set; }
    }
}
