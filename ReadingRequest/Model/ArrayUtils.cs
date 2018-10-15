using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public static class ArrayUtils
    {
        public static T[] ElementBetween<T>(this IEnumerable<T> data, int fromIndex, int toIndex)
        {
            List<T> resultList = new List<T>();
            for (int i = fromIndex; i <= toIndex; i++)
            {
                resultList.Add(data.ElementAt(i));
            }
            return resultList.ToArray();
        }

        public static decimal ToDecimal<T>(this IEnumerable<T> data) where T : struct
        {
            string hexValue = string.Empty;
            foreach (var item in data)
            {
                hexValue += int.Parse(item.ToString()).ToString("X2");
            }
            return int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
        }

        public static string JoinIntoString<T>(this IEnumerable<T> data) where T : struct
        {
            return string.Join("", data);
        }

        public static string ToHexString<T>(this IEnumerable<T> data) where T : struct
        {
            return string.Join("", data.Select(e => int.Parse(e.ToString()).ToString("X2")));
        }
    }
}
