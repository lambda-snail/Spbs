using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Integrations.Nordigen.Models;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

enum SelectionState
{
    SelectInstitution,
    CreateEula
}

public partial class BankSyncronizationPage
{
    private InstitutionSelectorComponent _institutionSelector;
    private SelectionState _selectionState = SelectionState.SelectInstitution;

    protected override void OnInitialized()
    {
    }

    public async Task OnContinueButtonClicked()
    {
        _selectionState = SelectionState.CreateEula;
        StateHasChanged();
    }
}