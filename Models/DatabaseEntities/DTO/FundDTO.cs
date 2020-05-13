using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    [Serializable]
    public class FundDTO : BaseEntity
    {
        
        public string name_text { get; set; }
       
        public string description { get; set; }

        public DateTime? date_time_added { get; set; }
        public Guid? id_user { get; set; }
    }
}
