using MongoDB.Bson.Serialization.Attributes;
using Spbs.Main.Core.Models;

namespace Spbs.Main.InfraStructure.DtoModels;

public class LocationDto
{
    public Guid Id { get; set; }
    [BsonElement("userId")]
    public Guid UserId { get; set; }
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("parentId")]
    public Guid? ParentId { get; set; }
}