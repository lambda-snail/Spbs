using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Spbs.Ui.Features.BankIntegration.Models;

/// <summary>
/// A representation of what Nordigen calls a 'link' - i.e. a connection to an institution for a specific user.
/// </summary>
public class NordigenRequisition 
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("nordigenId")]
    public Guid NordigenId { get; set; }
    
    [JsonProperty("userId")]
    public Guid UserId { get; set; }
    
    [JsonProperty("created")]
    public DateTime? Created { get; set; }

    /// <summary>
    /// An Institution ID for this Requisition
    /// </summary>
    /// <value>an Institution ID for this Requisition</value>
    [JsonProperty("institutionId")]
    public string InstitutionId { get; set; }

    /// <summary>
    /// EULA associated with this requisition
    /// </summary>
    /// <value>EUA associated with this requisition</value>
    [JsonProperty("agreement")]
    public Guid Agreement { get; set; }

    /// <summary>
    /// Array of account IDs retrieved within a scope of this requisition
    /// </summary>
    [JsonProperty("accounts")]
    public List<Guid> Accounts { get; private set; }

    /// <summary>
    /// A two-letter country code (ISO 639-1)
    /// </summary>
    [JsonProperty("language")]
    public string UserLanguage { get; set; }

    [JsonProperty("authorizationLink")]
    public string AuthorizationLink { get; private set; }
}