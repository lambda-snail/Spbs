#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.Users;

public partial class UserSettingsPage : ComponentBase
{
    [Parameter]
    public string? Area { get; set; }

    private static readonly Type _defaultSettingsPage = typeof(UserProfileComponent);
    private Dictionary<string, Type> _settingsPageMap = new();

    private bool _readyToRender = false;
    private DynamicComponent _dynamicComponent;
    private Dictionary<string, object> _dynamicComponentParameters = new();

    protected override void OnInitialized()
    {
        _settingsPageMap = new()
        {
            { "profile", typeof(UserProfileComponent) },
            { "locale", typeof(UserLocaleSettingsComponent) }
        };
        
        _dynamicComponentParameters.Add("UserObject", new User { TestName = "Hello World!!"});

        _readyToRender = true;
        StateHasChanged();
    }
}