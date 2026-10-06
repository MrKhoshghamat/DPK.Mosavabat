using DPK.Mosavabat.Common.Enums;
using System.Globalization;

namespace DPK.Mosavabat.Common.Extensions
{
    public static class DateTimeExtensions
    {
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
    }
}
