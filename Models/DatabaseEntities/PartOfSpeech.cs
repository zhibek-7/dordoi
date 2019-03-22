using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class PartOfSpeech : BaseEntity
    {
        public Guid ID_Locale { get; set; }

        public string Name_text { get; set; }

    }
}
