using System.Globalization;

namespace DPK.Mosavabat.Common.Extensions;

public static class IntExtension
{
    public static string ToFixLengthWithZero(this decimal input, int number)
    {
        return input.ToString(CultureInfo.InvariantCulture).PadLeft(number, '0');
    }
}