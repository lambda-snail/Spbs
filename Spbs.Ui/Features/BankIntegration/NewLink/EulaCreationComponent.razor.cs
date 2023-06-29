using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shared.Utilities;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.ComponentServices;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration.NewLink;

[AuthenticationTaskExtension()]
public partial class EulaCreationComponent : ComponentBase
{
    private NordigenEula _eula = new();
    
#pragma warning disable CS8618
    [Inject] private IEulaService _eulaService { get; set; }
    [Inject] private IDateTimeProvider _dateTime { get; set; }
    [Inject] private INotificationService _notificationService { get; set; } 

    [Parameter, Required] public Func<Institution> SetInstitution { get; set; }
    private Institution? _institution = null;

    [Parameter, Required] public Func<Task> OnEulaCreatedCallbackAsync { get; set; }
#pragma warning restore CS8618
    
    private bool _isSubmitted = false;

    public NordigenEula? GetEula()
    {
        return _isSubmitted ? _eula : null;
    }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        Guid? userId = await UserId();
        if (userId is null) { return; } // Something is wrong
        
        var now = _dateTime.Now();
        _eula.Created = now;
        _eula.Accepted = now;
        _eula.UserId = userId.Value;
        
        _institution = SetInstitution();
        _eula.InstitutionId = _institution.Id;
    }

    private async Task HandleValidSubmit()
    {
        //await _eulaService.UpsertEula(_eula);
        _isSubmitted = true;

        _eula = await _eulaService.CreateEulaWithNordigen(_eula);
        _notificationService.ShowToast("Eula Created", "An agreement has been created. You must accept this in the authentication step to proceed.", NotificationLevel.Success);
        await OnEulaCreatedCallbackAsync.Invoke()!;
    }

    private async Task HandleInvalidSubmit()
    {

    }
}