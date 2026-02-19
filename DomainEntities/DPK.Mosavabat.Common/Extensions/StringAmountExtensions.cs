using System.Globalization;

namespace DPK.Mosavabat.Common.Extensions;

public static class StringAmountExtensions
{
    public static string ToThousandSeparatedWithOutDecimals(this string stringAmount)
    {
        if (string.IsNullOrEmpty(stringAmount))
            return stringAmount;

        if (decimal.TryParse(stringAmount, out var amount))
        {
            amount = Math.Truncate(amount);
            return amount.ToString("N0", CultureInfo.InvariantCulture);
        }
        else
            throw new FormatException("Input string is not valid");
    }
}