﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
  public  class LocalizationProjectsLocales 
    {
        [Required]
        public int ID_Localization_Project { get; set; }
        [Required]
        public int ID_Locale { get; set; }
        [Required]
        public float Percent_Of_Translation { get; set; }
        [Required]
        public float Percent_Of_Confirmed { get; set; }
    }
}
