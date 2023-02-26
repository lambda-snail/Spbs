using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Integrations.Nordigen.Models;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public partial class BankSyncronizationPage
{
    private InstitutionSelectorComponent _institutionSelector;
    

    protected override void OnInitialized()
    {
        
    }

}