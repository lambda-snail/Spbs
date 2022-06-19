using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Spbs.Main.InfraStructure.DtoModels;

namespace Spbs.Main.InfraStructure.Utilities;

public static class BsonClassMapRegistrator
{
    public static void RegisterBsonClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(PurchaseDto)))
        {
            BsonClassMap.RegisterClassMap<PurchaseDto>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(purchase => purchase.Id);
                cm.IdMemberMap.SetIgnoreIfDefault(true);
                cm.IdMemberMap.SetIdGenerator(AscendingGuidGenerator.Instance);
            });
        }
        
        if (!BsonClassMap.IsClassMapRegistered(typeof(PurchaseItemDto)))
        {
            BsonClassMap.RegisterClassMap<PurchaseItemDto>(cm =>
            {
                cm.AutoMap();
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(UserDto)))
        {
            BsonClassMap.RegisterClassMap<UserDto>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(user => user.Id);
                cm.IdMemberMap.SetIgnoreIfDefault(true);
            });
        }
        
        if (!BsonClassMap.IsClassMapRegistered(typeof(UserSettingsDto)))
        {
            BsonClassMap.RegisterClassMap<UserSettingsDto>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(tz => tz.TimeZone.Id);
                cm.MapMember(s => s.TimeZone).SetSerializer(new TimeZoneInfoSerializer());
            });
        }
    }
}