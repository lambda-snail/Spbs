using System;
using Newtonsoft.Json;

namespace Spbs.Ui.Data.Cosmos;

public class CosmosDocument<T>
{
    [JsonProperty("id")]
    public required Guid Id { get; set; }

    [JsonProperty("type")] 
    public required string Type { get; set; }
    [JsonProperty("data")]
    public required T Data { get; set; }

    [JsonProperty("_ttl")] 
    public int? Ttl { get; set; }
    [JsonProperty("_etag")] 
    public string? Etag { get; set; }
}