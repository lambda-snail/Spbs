using System;
using Newtonsoft.Json;
using Spbs.Ui.Data.Cosmos;

namespace Spbs.Ui.Features.Users;

public class User : ICosmosData
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public LocaleInformation LocaleInformation { get; set; }

    public DateTime ModifiedOn { get; set; }
}

public struct LocaleInformation
{
    [JsonConverter(typeof(TimeZoneInfoZerializer))]
    public TimeZoneInfo TimeZone { get; set; }
    
    public DateTime ToUserTimeZone(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZone);
    }

    public DateTime ToUtcAsync(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZone);
    }
}