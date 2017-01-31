using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string entry, string compare, bool caseInsensitive)
        {
            if (entry == null && compare == null)
            {
                return true;
            }
            if (entry == null || compare == null)
            {
                return false;
            }

            return caseInsensitive ? entry.ToLower().Contains(compare.ToLower()) : entry.Contains(compare);
        }
    }
}
