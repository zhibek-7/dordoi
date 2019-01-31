using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
  public  class LocalizationProjectsLocales : BaseEntity
    {
        [Required]
        public int ID_LocalizationProject { get; set; }
        [Required]
        public int ID_Locale { get; set; }
        [Required]
        public float PercentOfTranslation { get; set; }
        [Required]
        public float PercentOfConfirmed { get; set; }
    }
}
