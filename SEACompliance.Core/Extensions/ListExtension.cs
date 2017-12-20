using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Core.Extensions
{
    public static class ListExtension
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortString = "Rank", string direction = "asc")
        {
            if (list != null && list.Count() > 1)
            {
                var prop = typeof(T).GetProperty(sortString);
                if (direction.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                    return list.OrderByDescending(l => prop.GetValue(l, null)).ToList();
                else
                    return list.OrderBy(l => prop.GetValue(l, null)).ToList();
            }
            return list;
        }
    }
}
