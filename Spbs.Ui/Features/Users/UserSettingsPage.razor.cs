#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Users.Repositories;

namespace Spbs.Ui.Features.Users;

[AuthenticationTaskExtension]
public partial class UserSettingsPage : ComponentBase
{
    [Parameter] public string? Area { get; set; }

#pragma warning disable CS8618
    [Inject] private IUserRepository _userRepository { get; set; }
    
    private DynamicComponent _dynamicComponent;
#pragma warning restore CS8618
    
    private static readonly Type _defaultSettingsPage = typeof(UserProfileComponent);
    private Dictionary<string, Type> _settingsPageMap = new();

    private bool _readyToRender = false;
    private readonly Dictionary<string, object> _dynamicComponentParameters = new();

    private User? _user;
    
    protected override async Task OnInitializedAsync()
    {
        _settingsPageMap = new()
        {
            { "profile", typeof(UserProfileComponent) },
            { "locale", typeof(UserLocaleSettingsComponent) }
        };

        Guid? userId = await UserId();
        _user = await _userRepository.GetById(_userId!.Value);
        if (_user is null)
        {
            return;
        }

        _dynamicComponentParameters.Add("UserObject", _user);
        _dynamicComponentParameters.Add("UserSettingsChangedCallback", UserProfileSettings_UserSettingsChanged);
        
        _readyToRender = true;
        StateHasChanged();
    }

    private async Task UserProfileSettings_UserSettingsChanged()
    {
        //await _userRepository.UpsertUser(user);
        Console.WriteLine("Hello!");
    }
}