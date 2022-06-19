namespace Spbs.Main.Core.Settings;

public class SpbsDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string PurchasesCollection { get; set; }
    public string UsersCollection { get; set; }
}