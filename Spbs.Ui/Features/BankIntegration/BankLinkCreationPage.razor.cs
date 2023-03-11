using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Integrations.Nordigen.Models;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.ComponentServices;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

enum SelectionState
{
    SelectInstitution,
    CreateEula
}

public partial class BankLinkCreationPage
{
    [Inject] private NotificationService NotificationService { get; set; }
    
    private InstitutionSelectorComponent _institutionSelector;
    private SelectionState _selectionState = SelectionState.SelectInstitution;

    protected override void OnInitialized()
    {
    }

    public async Task OnContinueButtonClicked()
    {
        switch (_selectionState)
        {
            case SelectionState.SelectInstitution:
                TrySetState_CreateEula();
                break;
        }
        
        StateHasChanged();
    }

    public void TrySetState_CreateEula()
    {
        if (!_institutionSelector.HasSelection())
        {
            NotificationService.ShowToast("No Institution Selected", "Please select an institution before proceeding.", NotificationLevel.Warning);            
            return;
        }
        
        _selectionState = SelectionState.CreateEula;
    }
}