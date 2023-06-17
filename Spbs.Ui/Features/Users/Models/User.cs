using System;
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
    public TimeZoneInfo TimeZone { get; set; }
}