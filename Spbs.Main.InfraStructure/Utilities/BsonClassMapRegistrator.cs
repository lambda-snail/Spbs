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
        
        if (!BsonClassMap.IsClassMapRegistered(typeof(PurchaseDto)))
        {
            BsonClassMap.RegisterClassMap<PurchaseItemDto>(cm =>
            {
                cm.AutoMap();
            });
        }
    }
}