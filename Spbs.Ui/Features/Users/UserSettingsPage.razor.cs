#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Spbs.Ui.Features.Users;

public partial class UserSettingsPage : ComponentBase
{
    private Type? _userSettingsComponentType = null;

    [Parameter]
    public string? Area { get; set; }

    private static readonly Type _defaultSettingsPage = typeof(UserProfileComponent);
    private Dictionary<string, Type> _settingsPageMap = new()
    {
        { "profile", typeof(UserProfileComponent) },
        { "locale", typeof(UserLocaleSettingsComponent) }
    };
}