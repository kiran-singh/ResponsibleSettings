using System;

namespace Library.Standard
{
    public static class StringExtensions
    {
        public static bool ToBool(this string input)
        {
            bool.TryParse(input, out var result);

            return result;
        }

        public static DateTime ToDateTime(this string input)
        {
            DateTime.TryParse(input, out var result);

            return result;
        }

        public static Guid ToGuid(this string input)
        {
            Guid.TryParse(input, out var result);

            return result;
        }

        public static int ToInt(this string input)
        {
            int.TryParse(input, out var result);

            return result;
        }
    }
}