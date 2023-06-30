using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spbs.Data.Cosmos;

namespace Spbs.Ui.Features.BankIntegration.Models;

public class User: ICosmosData
{
    [JsonProperty("id")] 
    public Guid Id { get; set; }

    /// <summary>
    /// The user id in azure ad.
    /// </summary>
    public Guid UserId { get; set; }

    [JsonProperty("nordigenLinks")] 
    public List<Guid> NordigenLinks { get; set; } = new ();
}