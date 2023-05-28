using System;
using Newtonsoft.Json;

namespace Spbs.Ui.Data.Cosmos;

public interface ICosmosData
{
    [JsonProperty("id")]
    Guid Id { get; set; }
    
    [JsonProperty("userId")]
    Guid UserId { get; set; }
}

public class CosmosDocument<T> where T: ICosmosData
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