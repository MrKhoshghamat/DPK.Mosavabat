using System.Globalization;

namespace DPK.Mosavabat.Services.Extensions
{
    public static class StringAmountExtensions
    {
        public static string ToThousandSeparetedWithOutDecimals(this string stringAmount)
        {
            if (String.IsNullOrEmpty(stringAmount))
                return stringAmount;

            if (decimal.TryParse(stringAmount, out decimal amount))
            {
                amount = Math.Truncate(amount);
                return amount.ToString("N0", CultureInfo.InvariantCulture);
            }
            else
                throw new FormatException("Input string is not valid");
        }
    }
}
