using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Data
{
    public class ConvertData
    {
        public static List<Guid> ConverLocale(IEnumerable<Guid> loc_int)
        {
            //var loc_int = glossary.Locales_Ids;
            List<Guid> loc = new List<Guid>();
            foreach (var id in loc_int)
            {
                if (id != null)
                {
                    loc.Add(id);
                }
            }

            return loc;
        }
    }
}
