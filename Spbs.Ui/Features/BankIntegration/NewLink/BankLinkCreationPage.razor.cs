using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.ComponentServices;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.NewLink;

enum SelectionState
{
    SelectInstitution,
    CreateEula,
    CreateLink,
    Done
}

internal class SelectionViewModel
{
    public SelectionState State { get; set; }
    public Institution Institution { get; set; }
    public NordigenEula Eula { get; set; }
}

public partial class BankLinkCreationPage
{
    [Inject] private INotificationService _notificationService { get; set; }
    [Inject] private NavigationManager _navigationManager { get; set; }
    
    private InstitutionSelectorComponent _institutionSelector;
    private EulaCreationComponent _eulaCreator;
    private NewLinkComponent _newLink;
    
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
            case SelectionState.CreateEula:
                TrySetState_CreateLink();
                break;
            case SelectionState.CreateLink:
                await TrySetState_Done();
                break;
        }
        
        StateHasChanged();
    }

    public void TrySetState_CreateEula()
    {
        var institution = _institutionSelector.GetSelectedInstitution();
        if (!_institutionSelector.HasSelection() || institution is null)
        {
            _notificationService.ShowToast("No Institution Selected", "Please select an institution before proceeding.", NotificationLevel.Warning);            
            return;
        }

        _selectionData.Institution = institution!;
        _selectionData.State = SelectionState.CreateEula;
    }

    public void TrySetState_CreateLink()
    {
        var maybeEula = _eulaCreator.GetEula();
        if(maybeEula is not { } eula )
        {
            return;
        }

        _selectionData.Eula = eula;
        _selectionData.State = SelectionState.CreateLink;
    }

    public async Task TrySetState_Done()
    {
        var redirect = await _newLink.CreateLink();
        if (redirect is null)
        {
            _notificationService.ShowToast("Error", "Unable to link to your bank.", NotificationLevel.Error);
            return;
        }
        
        _navigationManager.NavigateTo(redirect.Value.Url);
    }
}