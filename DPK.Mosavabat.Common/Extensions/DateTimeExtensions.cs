using DPK.Mosavabat.Common.Enums;
using System.Globalization;

namespace DPK.Mosavabat.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly string[] Formats = { "yyyy/MM/dd" };

        public static string ToFormattedPersianDate(this DateTime dateTime, DateFormat format)
        {
            var persianCalendar = new PersianCalendar();

            // Get Persian year, month, and day  
            var persianYear = persianCalendar.GetYear(dateTime);
            var persianMonth = persianCalendar.GetMonth(dateTime);
            var persianDay = persianCalendar.GetDayOfMonth(dateTime);

            // Format the date as a string based on the specified format  
            var formattedDate = format switch
            {
                DateFormat.TwoDigitYear => $"{persianYear % 100:D2}{persianMonth:D2}{persianDay:D2}",
                DateFormat.FourDigitYear => $"{persianYear:D4}{persianMonth:D2}{persianDay:D2}",
                _ => throw new ArgumentException("Invalid date format specified")
            };

            return formattedDate;
        }

        public static string MiladiToShamsi(this string input)
        {
            var miladiDate = DateTime.ParseExact(input, Formats, CultureInfo.InvariantCulture);
            var pc = new PersianCalendar();
            var year = pc.GetYear(miladiDate);
            var month = pc.GetMonth(miladiDate);
            var day = pc.GetDayOfMonth(miladiDate);

            return $"{year:0000}/{month:00}/{day:00}";
        }

        public static bool IsGregorianDate(this string input)
        {
            var isGregorian = DateTime.TryParseExact(
                input,
                Formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date
            );
            if (isGregorian)
                return date.Year > 1600;
            return false;
        }
    }
}
