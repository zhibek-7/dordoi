using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    [Serializable]
    public class FundDTO// : BaseEntity
    {
        
        public string fund_text { get; set; }

       
        public string fund_description { get; set; }

        public DateTime? data_create { get; set; }
        public Guid ID_User { get; set; }
    }
}
