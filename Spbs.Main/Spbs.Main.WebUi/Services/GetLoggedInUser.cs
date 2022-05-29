using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Spbs.Main.WebUi.Contracts;

namespace Spbs.Main.WebUi.Services;

public class LoggedInUserService : ILoggedInUserService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IMediator _mediator;

    public LoggedInUserService(AuthenticationStateProvider authenticationStateProvider,
        IMediator mediator)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _mediator = mediator;
    }
    
    // public async Task<User> GetLoggedInUser()
    // {
    //     Guid userId = await GetLoggedInUserId();
    //     return await _mediator.Send(new GetUserByIdRequest { UserId = userId });
    // }

    public async Task<Guid> GetLoggedInUserId()
    {
        AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        string userId = authenticationState.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId);
    }
}