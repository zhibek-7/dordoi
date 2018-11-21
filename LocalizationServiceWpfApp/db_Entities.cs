using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationServiceWpfApp
{
    public partial class db_Entities : DbContext
    {
        public db_Entities() : base(nameOrConnectionString: "LocaliztionService") { }

        public DbSet<LSFile> LSFile { get; set; }
        public DbSet<LSString> LSString { get; set; }
    }
}
