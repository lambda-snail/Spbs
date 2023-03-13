using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.NewLink;

public partial class NewLinkComponent : ComponentBase
{
    [Parameter, Required] public Func<Institution> SetInstitution { get; set; }
    private Institution? _institution = null;
    
    [Parameter, Required] public Func<NordigenEula> SetEula { get; set; }
    private NordigenEula? _eula = null;

    protected override void OnInitialized()
    {
        _institution = SetInstitution();
        _eula = SetEula();
        
    }
}