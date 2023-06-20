#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components.UserSettings;
using Spbs.Ui.ComponentServices;
using Spbs.Ui.Features.Expenses;
using Spbs.Ui.Features.Users.Repositories;

namespace Spbs.Ui.Features.Users;

[AuthenticationTaskExtension]
public partial class UserSettingsPage : ComponentBase
{
    [Parameter] public string? Area { get; set; }

#pragma warning disable CS8618
    [Inject] private IUserRepository _userRepository { get; set; }
    [Inject] private INotificationService _notificationService { get; set; }

    private DynamicComponent _dynamicComponent;
#pragma warning restore CS8618

    private static readonly Type _defaultSettingsPage = typeof(UserProfileComponent);
    private Dictionary<string, SettingsMenuEntryData> _settingsPageMap = new();

    private bool _readyToRender = false;
    private readonly Dictionary<string, object> _dynamicComponentParameters = new();

    private User? _user;

    protected override async Task OnInitializedAsync()
    {
        InitSettingsComponentMap();

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

    private struct SettingsMenuEntryData
    {
        public Type ComponentType { get; set; }
        public string MenuName { get; set; }
    }

    private void InitSettingsComponentMap()
    {
        _settingsPageMap = new()
        {
            {
                "profile",
                new()
                {
                    ComponentType = typeof(UserProfileComponent),
                    MenuName = UserSettingsComponentBase.GetMenuName<UserProfileComponent>()
                }
            },
            {
                "locale",
                new()
                {
                    ComponentType = typeof(UserLocaleSettingsComponent),
                    MenuName = UserSettingsComponentBase.GetMenuName<UserLocaleSettingsComponent>()
                }
            },
            {
                "expenses", 
                new()
                {
                 ComponentType = typeof(ExpenseSettingsComponent),
                 MenuName = UserSettingsComponentBase.GetMenuName<ExpenseSettingsComponent>()
                }
            }
        };
    }

    private async Task UserProfileSettings_UserSettingsChanged()
    {
        if (_user is not null)
        {
            await _userRepository.UpsertUser(_user);
            _notificationService.ShowToast("Save successful", "Your settings have been saved!",
                NotificationLevel.Success);
        }
    }
}