using MongoDB.Bson.Serialization.Attributes;
using Spbs.Main.InfraStructure.Utilities;

namespace Spbs.Main.InfraStructure.DtoModels;

public class UserDto
{
    public Guid Id { get; set; }
    public UserSettingsDto Settings { get; set; }
}

public class UserSettingsDto
{
    public string TimeZone { get; set; }
}