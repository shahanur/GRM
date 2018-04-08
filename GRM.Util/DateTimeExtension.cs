using System;
using System.Collections.Generic;
using System.Globalization;

namespace GRM.Util
{
    public static class DateTimeExtension
    {

        private static readonly IDictionary<int, string> _monthDescriptor = new Dictionary<int, string>
        {
            {1,"Jan" },
            {2,"Feb" },
            {3,"Mar" },
            {4,"April" },
            {5,"May" },
            {6,"June" },
            {7,"Jul" },
            {8,"Aug" },
            {9,"Sep" },
            {10,"Oct" },
            {11,"Nov" },
            {12,"Dec" }
        };

        private static string DayOrdinalizer(int day)
        {
            var ordinal = "th";
            switch (day)
            {
                case 1:
                case 31:
                    ordinal = "st";
                    break;
                case 2:
                    ordinal = "nd";
                    break;

                case 3:
                    ordinal = "rd";
                    break;
            }

            return day + ordinal;
        }

        public static string GetOrdinalized(this DateTime? dateTime)
        {
            if (null == dateTime) return string.Empty;
            return string.Format("{0} {1} {2}", DayOrdinalizer(dateTime.Value.Day), _monthDescriptor[dateTime.Value.Month],
                dateTime.Value.Year);
        }

        public static DateTime? GetDateTimeFromOrdinal(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;
            var provider = CultureInfo.InvariantCulture;
            var format = "d MMM yyyy";
            
                if (stringValue.Split(' ')[1].Length == 4)
                    format = "d MMMM yyyy";
            if (stringValue.Split(' ')[1].Length == 5)
                format = "d MMMMM yyyy";
            return DateTime.ParseExact(
                        stringValue.ToLower().Replace("st", string.Empty).Replace("nd",string.Empty).Replace("rd",string.Empty).Replace("th",string.Empty),
                        format, provider);
        }
    }


}
