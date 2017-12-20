using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEACompliance.Core.Extensions
{
    public static class ConvertExtension
    {
        public static int ToInt32(this string value)
        {
            int result;
            if (!Int32.TryParse(value, out result))
            {
                result = 0;
            }
            return result;

        }

        public static string ToFormatString(this DateTime? date, string pattern)
        {
            return date.HasValue ? date.Value.ToString(pattern) : string.Empty;
        }

        public static string ToTangibleDateString(this string source)
        {
            if (string.IsNullOrEmpty(source)) return source;
            if (!new Regex(@"^(\d{2}\/){2}\d{4}$").IsMatch(source)) throw new ArgumentException("Invalid param pattern");
            var parts = source.Split('/');
            string day = parts[0], month = parts[1], year = parts[2];
            day = GetDayOrdinal(day);
            month = GetMonthOrdinal(month);
            return string.Join(" ", day, month, year);
        }

        private static string GetMonthOrdinal(string month)
        {
            var array = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var index = int.Parse(month);
            return array[index - 1];
        }

        private static string GetDayOrdinal(string day)
        {
            var array = new string[] { "1st", "2nd", "3rd" };
            var index = int.Parse(day);
            return index > 3 ? (day.TrimStart('0') + "th") : array[index - 1];
        }

    }
}
