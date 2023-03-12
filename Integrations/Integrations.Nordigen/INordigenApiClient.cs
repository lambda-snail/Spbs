using Integrations.Nordigen.Models;

namespace Integrations.Nordigen;

public interface INordigenApiClient
{
    /// <summary>
    /// Retrieve the list of supported institutions for the given country, or an empty list if no institutions exist.
    /// </summary>
    Task<List<Aspsp>> GetListOfInstitutionsAsync(string country);
    Task<Aspsp?> GetInstitutionAsync(string id);

    /// <summary>
    /// Attempt to create an end user agreement based on the provided details. If the agreement was created, the agreement id
    /// will be populated in the return value. This id should be used in all future communication to Nordigen about this
    /// particular agreement. 
    /// </summary>
    Task<EndUserAgreement?> CreateEndUserAgreement(
        string institutionId,
        int maxHistoricalDays,
        int accessValidForDays,
        List<string> accessScope);

}