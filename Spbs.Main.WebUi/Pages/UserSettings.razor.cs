using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;
using Spbs.Main.WebUi.Contracts;
using Spbs.Main.WebUi.ViewModels;

namespace Spbs.Main.WebUi.Pages;

[Authorize]
public partial class UserSettings
{
    [Inject] public IMapper Mapper { get; set; }
    [Inject] public IMediator Mediator { get; set; }
    [Inject] public ILoggedInUserService UserService { get; set; }
    
    private UserViewModel? User { get; set; } = null;

    private IEnumerable<TimeZoneInfo> TimeZones;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Guid userId = await UserService.GetLoggedInUserId();
        
        var response = await Mediator.Send(new GetUserDetails.Request(userId));
        if (response.Success)
        {
            User = Mapper.Map<UserViewModel>(response.User);
        }
        else
        {
            CreateNewUserWithDefaultSettings(userId);
        }

        TimeZones = Core.Models.User.GetAvailableTimeZones();
        StateHasChanged();
    }

    private void CreateNewUserWithDefaultSettings(Guid userId)
    {
        User = new UserViewModel();
        User.Id = userId;
        User.Settings = new UserSettingsViewModel();
        User.Settings.TimeZone = TimeZoneInfo.Utc.Id;
    }

    private async Task HandleValidSubmit()
    {
        User toUpdate = Mapper.Map<User>(User); 
        await Mediator.Send(new UpsertUserDetails.Request(User: toUpdate!));
    }

    private void HandleInvalidSubmit()
    {
        
    }
}