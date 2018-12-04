using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    public class PartOfSpeech : BaseEntity
    {

        [Key]
        public int ID { get; set; }

        public int ID_Locale { get; set; }

        public string Name { get; set; }

    }
}
