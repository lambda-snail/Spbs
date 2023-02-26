using Integrations.Nordigen.Models;

namespace Integrations.Nordigen;

public interface INordigenApiClient
{
    /// <summary>
    /// Retrieve the list of supported institutions for the given country, or an empty list if no institutions exist.
    /// </summary>
    Task<List<Aspsp>> GetListOfInstitutionsAsync(string country);

    Task<Aspsp?> GetInstitutionAsync(string id);
}