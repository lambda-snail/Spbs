using System;
using System.Globalization;
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

public class LocaleInformation
{
    [JsonConverter(typeof(TimeZoneInfoSerializer))]
    public TimeZoneInfo TimeZone { get; set; }

    [JsonConverter(typeof(CultureInfoSerializer))]
    public CultureInfo CultureInfo { get; set; }
    
    public DateTime ToUserTimeZone(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZone);
    }

    public DateTime ToUtcAsync(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZone);
    }
}