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

internal class SelectionViewModel
{
    public SelectionState State { get; set; }
    public Institution Institution { get; set; }
}

public partial class BankLinkCreationPage
{
    [Inject] private NotificationService NotificationService { get; set; }
    
    private InstitutionSelectorComponent _institutionSelector;
    private EulaCreationComponent _eulaCreator;
    
    private SelectionViewModel _selectionData = new() { State = SelectionState.SelectInstitution };
    
    protected override void OnInitialized()
    {
    }

    public async Task OnContinueButtonClicked()
    {
        switch (_selectionData.State)
        {
            case SelectionState.SelectInstitution:
                TrySetState_CreateEula();
                break;
        }
        
        StateHasChanged();
    }

    public void TrySetState_CreateEula()
    {
        var institution = _institutionSelector.GetSelectedInstitution();
        if (!_institutionSelector.HasSelection() || institution is null)
        {
            NotificationService.ShowToast("No Institution Selected", "Please select an institution before proceeding.", NotificationLevel.Warning);            
            return;
        }

        _selectionData.Institution = institution!;
        _selectionData.State = SelectionState.CreateEula;
    }
}