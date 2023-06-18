using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Spbs.Ui.Features.Users;

public class CultureInfoSerializer : JsonConverter<CultureInfo>
{
    public override void WriteJson(JsonWriter writer, CultureInfo? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.Name);
    }

    public override CultureInfo? ReadJson(JsonReader reader, Type objectType, CultureInfo? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return reader.Value is string json ? CultureInfo.CreateSpecificCulture(json) : null;
    }
}