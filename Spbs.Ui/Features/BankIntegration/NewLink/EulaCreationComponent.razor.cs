using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Utilities;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;
using Severity = MudBlazor.Severity;

namespace Spbs.Ui.Features.BankIntegration.NewLink;

[AuthenticationTaskExtension()]
public partial class EulaCreationComponent : ComponentBase
{
    private NordigenEula _eula = new();
    
#pragma warning disable CS8618
    [Inject] private IEulaService _eulaService { get; set; }
    [Inject] private IDateTimeProvider _dateTime { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }
    [Inject] private IValidator<NordigenEula> _eulaValidator { get; set; }

    [Parameter, Required] public Func<Task> OnEulaCreatedCallbackAsync { get; set; }
    [Parameter, Required] public Func<Institution> SetInstitution { get; set; }
    private Institution? _institution = null;
    private MudForm _form;
#pragma warning restore CS8618
    
    private bool _isSubmitted = false;
    private string _scopesFormBindDummy = string.Empty;

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
        var validationResult = await _eulaValidator.ValidateAsync(_eula);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _snackbar.Add(error.ErrorMessage, Severity.Error);
            }
            
            _snackbar.Add("Attempted to create a eula but encountered errors. Please address them and try again.", Severity.Warning);
            return;
        }
        
        _isSubmitted = true;

        _eula = await _eulaService.CreateEulaWithNordigen(_eula);
        _snackbar.Add("An agreement has been created. You must accept this in the authentication step to proceed.", Severity.Success);
        await OnEulaCreatedCallbackAsync.Invoke()!;
    }

    private void OnScopeSelectionChanged(IEnumerable<string> selection)
    {
        _eula.AccessScope = selection.ToArray();
    }
}