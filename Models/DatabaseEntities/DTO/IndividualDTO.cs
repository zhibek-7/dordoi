using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    [Serializable]
    public class IndividualDTO : BaseEntity
    {
       
        public string name_text_first { get; set; }

      
        public string description_first { get; set; }



      
        public string name_text_second { get; set; }

        
        public string description_second { get; set; }




        
        public string name_text_third { get; set; }

      
        public string description_third { get; set; }




     
        public string name_text_fourth { get; set; }

      
        public string description_fourth { get; set; }






        
        public string name_text_fifth { get; set; }

      
        public string description_fifth { get; set; }





        public DateTime? data_create { get; set; }
        public Guid ID_User { get; set; }
    }
}
