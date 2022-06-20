namespace Spbs.Main.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public UserSettings Settings { get; set; }

    public static IEnumerable<TimeZoneInfo> GetAvailableTimeZones()
    {
        return TimeZoneInfo.GetSystemTimeZones();
    }
}

public class UserSettings
{
    public TimeZoneInfo TimeZone { get; set; }

    public DateTime ToUserTimeZone(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZone);
    }

    public DateTime ToUtc(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZone);
    }
}