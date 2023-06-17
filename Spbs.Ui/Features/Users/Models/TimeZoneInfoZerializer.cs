using System;
using Newtonsoft.Json;

namespace Spbs.Ui.Features.Users;

public class TimeZoneInfoZerializer : JsonConverter<TimeZoneInfo>
{
    public override void WriteJson(JsonWriter writer, TimeZoneInfo? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.Id);
    }

    public override TimeZoneInfo? ReadJson(JsonReader reader, Type objectType, TimeZoneInfo? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        string? json = reader.Value as string;
        return json is not null ? TimeZoneInfo.FindSystemTimeZoneById(json) : null;
    }
}