using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Individual : BaseEntity
    {
        [Required]
        public string name_text_first { get; set; }

        [Required]
        public string description_first { get; set; }



        [Required]
        public string name_text_second { get; set; }

        [Required]
        public string description_second { get; set; }




        [Required]
        public string name_text_third { get; set; }

        [Required]
        public string description_third { get; set; }




        [Required]
        public string name_text_fourth { get; set; }

        [Required]
        public string description_fourth { get; set; }






        [Required]
        public string name_text_fifth { get; set; }

        [Required]
        public string description_fifth { get; set; }





        public DateTime? data_create { get; set; }
        public Guid ID_User { get; set; }
    }
}
