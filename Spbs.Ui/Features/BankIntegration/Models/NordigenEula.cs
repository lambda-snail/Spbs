using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;

namespace Spbs.Ui.Features.BankIntegration.Models;

public class NordigenEula : ICosmosData
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public Guid UserId { get; set; }
    //[JsonProperty("nordigenId")]
    //public Guid NordigenId { get; set; }
    
    /// <summary>
    /// The date &amp; time at which the end user agreement was created.
    /// </summary>
    [JsonProperty("created")]
    public DateTime Created { get; set; }

    /// <summary>
    /// Maximum number of days of transaction data to retrieve.
    /// </summary>
    [JsonProperty("maxHistoricalDays")]
    public int MaxHistoricalDays { get; set; } = 90;

    /// <summary>
    /// Number of days from acceptance that the access can be used.
    /// </summary>
    [JsonProperty("accessValidForDays")]
    public int AccessValidForDays { get; set; } = 90;

    /// <summary>
    /// Array containing one or several values of [&#39;balances&#39;, &#39;details&#39;, &#39;transactions&#39;]
    /// </summary>
    [JsonProperty("accessScope")]
    public string[] AccessScope { get; set; } = Array.Empty<string>(); 

    /// <summary>
    /// The date &amp; time at which the end user accepted the agreement.
    /// </summary>
    [JsonProperty("accepted")]
    public DateTime? Accepted { get; set; }

    /// <summary>
    /// An Institution ID for this EUA
    /// </summary>
    [JsonProperty("institutionId")]
    public string InstitutionId { get; set; }
}