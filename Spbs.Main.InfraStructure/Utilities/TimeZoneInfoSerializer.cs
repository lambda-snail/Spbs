using System.Security.Cryptography;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Spbs.Main.InfraStructure.Utilities;

public class TimeZoneInfoSerializer : SerializerBase<TimeZoneInfo>
{
    public override TimeZoneInfo Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var type = context.Reader.GetCurrentBsonType();
        if (type == BsonType.String)
        {
            string fieldData = context.Reader.ReadString();
            return TimeZoneInfo.FindSystemTimeZoneById(fieldData);
        }
        
        return TimeZoneInfo.Utc;
    }
}