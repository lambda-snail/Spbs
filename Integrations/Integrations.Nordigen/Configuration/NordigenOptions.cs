using System.Diagnostics.CodeAnalysis;

namespace Integrations.Nordigen.Configuration;

public class NordigenOptions
{
    public const string NordigenOptionsSectionName = "NordigenOptions";

    [MaybeNull] public string ClientId { get; set; }
    [MaybeNull] public string ClientSecret { get; set; }
    [MaybeNull] public string ServiceUrl { get; set; }
    [MaybeNull] public string CallbackUrl { get; set; }
    [MaybeNull] public List<string> AccessScope { get; set; }
    [MaybeNull] public string NewTokenEndpoint { get; set; }
    [MaybeNull] public string RefreshTokenEndpoint { get; set; }
    [MaybeNull] public string ListOfInstitutionsEndpoint { get; set; }
    [MaybeNull] public string CreateAgreementEndpoint { get; set; }
    [MaybeNull] public string RequisitionEndpoint { get; set; }
    
    [MaybeNull] public string AccountEndpoint { get; set; }

    [NotNull] public int? DefaultMaxHistoricalDays { get; set; }
    [NotNull] public int? DefaultAccessValidForDays { get; set; }
}