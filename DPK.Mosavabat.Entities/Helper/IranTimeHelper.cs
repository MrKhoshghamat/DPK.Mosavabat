namespace DPK.Mosavabat.Entities.Helper;

public static class IranTimeHelper
{
    public static readonly TimeZoneInfo IranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");

    public static DateTime ToIranTime(DateTime utcTime)
    {
        if (utcTime.Kind != DateTimeKind.Utc)
        {
            utcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        }
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, IranTimeZone);
    }

    public static DateTime ToIranTimeNow()
    {
        return ToIranTime(DateTime.Now);
    }
}