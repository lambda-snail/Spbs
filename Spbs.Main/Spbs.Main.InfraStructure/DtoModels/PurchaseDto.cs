#nullable disable

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Spbs.Main.InfraStructure.DtoModels;

public class PurchaseDto
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.String)]
    public int Id { get; set; }
    [BsonElement("ownerId")]
    public string OwnerId { get; set; }
    [BsonElement("date")]
    public DateTime PurchaseDateTime { get; set; }
    [BsonElement("location")]
    public string Location { get; set; }
    [BsonElement("description")]
    public string Description { get; set; }
    [BsonElement("modelVersion")]
    public string ModelVersion { get; set; }
    [BsonElement("items")]

    public List<PurchaseItemDto> Items;
}