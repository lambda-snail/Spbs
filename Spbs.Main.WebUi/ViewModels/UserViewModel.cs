namespace Spbs.Main.WebUi.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public UserSettingsViewModel Settings { get; set; }
}

public class UserSettingsViewModel
{
    public string TimeZone { get; set; }
}