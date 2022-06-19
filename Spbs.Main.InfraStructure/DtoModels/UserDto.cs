using MongoDB.Bson.Serialization.Attributes;
using Spbs.Main.InfraStructure.Utilities;

namespace Spbs.Main.InfraStructure.DtoModels;

public class UserDto
{
    public Guid Id { get; set; }
    private UserSettingsDto Settings { get; set; }
}

public class UserSettingsDto
{
    public string TimeZoneId { get; set; }
}