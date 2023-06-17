using System;
using Newtonsoft.Json.Serialization;

namespace Spbs.Ui.Features.Users;

class SpbsContractResolver : DefaultContractResolver
{
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        JsonObjectContract contract = base.CreateObjectContract(objectType);
        if (objectType == typeof(TimeZoneInfo))
        {
            contract.Converter = new TimeZoneInfoSerializer();
        }

        return contract;
    }
}