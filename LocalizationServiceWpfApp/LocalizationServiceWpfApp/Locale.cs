using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationServiceWpfApp
{
    [Table("Strings", Schema = "public")]
    public class Locale
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Flag { get; set; }
    }
}
