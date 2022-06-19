namespace Spbs.Main.Core.Models;

public class User
{
    public Guid Id { get; set; }
    private UserSettings Settings { get; set; }
}

public class UserSettings
{
    public TimeZoneInfo TimeZone { get; set; }
}