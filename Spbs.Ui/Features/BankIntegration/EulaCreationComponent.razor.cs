using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Shared.Utilities;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration;

[AuthenticationTaskExtension()]
public partial class EulaCreationComponent : ComponentBase
{
    private NordigenEula _eula = new();
    
    [Inject] private IEulaService _eulaService { get; set; }
    [Inject] private IDateTimeProvider _dateTime { get; set; }

    [Parameter, Required] public Func<Institution> SetInstitution { get; set; }
    private Institution? _institution = null;
    
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
        await _eulaService.UpsertEula(_eula);
    }

    private async Task HandleInvalidSubmit()
    {

    }
}