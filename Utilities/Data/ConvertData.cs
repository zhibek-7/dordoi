using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Data
{
    public class ConvertData
    {
        public static List<int> ConverLocale(IEnumerable<int?> loc_int)
        {
            //var loc_int = glossary.Locales_Ids;
            List<int> loc = new List<int>();
            foreach (var id in loc_int)
            {
                if (id != null)
                {
                    loc.Add((int)id);
                }
            }

            return loc;
        }
    }
}
