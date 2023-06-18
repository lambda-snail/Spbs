using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
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
        else if (objectType == typeof(CultureInfo))
        {
            contract.Converter = new CultureInfoSerializer();
        }

        return contract;
    }
}