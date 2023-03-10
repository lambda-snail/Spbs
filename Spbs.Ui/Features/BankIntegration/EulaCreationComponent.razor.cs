using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public class EulaCreationViewModel
{
    
}

public partial class EulaCreationComponent : ComponentBase
{
    private NordigenEula _eula = new();

    [Inject] private INordigenApiClient NordigenClient { get; set; }

    private void HandleValidSubmit()
    {
        
    }

    private void HandleInvalidSubmit()
    {
        
    }
}