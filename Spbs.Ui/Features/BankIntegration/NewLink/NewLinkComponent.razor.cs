using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration.NewLink;

[AuthenticationTaskExtension()]
public partial class NewLinkComponent : ComponentBase
{
#pragma warning disable CS8618
    [Parameter, Required] public Func<Institution> SetInstitution { get; set; }
    private Institution? _institution = null;
    [Parameter, Required] public Func<NordigenEula> SetEula { get; set; }
    private NordigenEula? _eula = null;
    [Inject] private INordigenAccountLinkService _linkService { get; set; }
#pragma warning restore CS8618
    protected override void OnInitialized()
    {
        _institution = SetInstitution();
        _eula = SetEula();
    }

    public async Task<NordigenAccountLinkService.RedirectUrl?> CreateLink()
    {
        Guid? userId = await UserId();
        return await _linkService.CreateLink(_institution!, _eula!, userId!.Value, false);
    }
}
