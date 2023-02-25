namespace Integrations.Nordigen;

public class NordigenOptions
{
    public const string NordigenOptionsSectionName = "NordigenOptions";

    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    
    public string ServiceUrl { get; set; }
    public string CallbackUrl { get; set; }

    public List<string> AccessScope { get; set; }
    
    public string NewTokenEndpoint { get; set; }
    public string RefreshTokenEndpoint { get; set; }
    public string ListOfInstitutionsEndpoint { get; set; }
    
    public int? DefaultMaxHistoricalDays { get; set; }
    public int? DefaultAccessValidForDays { get; set; }
    
}