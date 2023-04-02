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

    /// <summary>
    /// Creates new link.
    /// </summary>
    /// <param name="redirect">Redirect URL to your application after end-user authorization with ASPSP (required).</param>
    /// <param name="institutionId">An Institution ID for this Requisition (required).</param>
    /// <param name="agreement">EUA associated with this requisition.</param>
    /// <param name="reference">Additional ID to identify the end user.</param>
    /// <param name="userLanguage">A two-letter country code (ISO 639-1).</param>
    /// <param name="ssn">Optional SSN field to verify ownership of the account.</param>
    /// <param name="accountSelection">Option to enable account selection view for the end user (default to false).</param>
    /// <param name="redirectImmediate">Enable redirect back to the client after account list received (default to false).</param>
    Task<RequisitionV2?> CreateRequisition(
        string redirect,
        string institutionId,
        Guid agreement,
        string reference,
        bool accountSelection,
        string userLanguage = "EN",
        bool redirectImmediate = false,
        string ssn = "");

    /// <summary>
    /// Deletes a requisition.
    /// </summary>
    Task DeleteRequisition(Guid requisitionId);
}