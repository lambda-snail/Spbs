using System;
using System.Threading.Tasks;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Shared.Utilities;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

[AuthenticationTaskExtension()]
public partial class EulaCreationComponent : ComponentBase
{
    private static Guid guid = Guid.NewGuid();
    private NordigenEula _eula = new() { Id = guid };

    [Inject] private INordigenApiClient _nordigenClient { get; set; }
    [Inject] private INordigenEulaWriterRepository _eulaWriterRepository { get; set; }
    [Inject] private INordigenEulaReaderRepository _eulaReaderRepository { get; set; }
    [Inject] private IDateTimeProvider _dateTime { get; set; }

    private async Task HandleValidSubmit()
    {
        Guid? userId = await UserId();
        if (userId is null) { return; } // Something is wrong
        
        var now = _dateTime.Now();
        _eula.Created = now;
        _eula.Accepted = now;
        _eula.UserId = userId.Value;
        await _eulaWriterRepository.Upsert(_eula);

        var eu = await _eulaReaderRepository.GetEulaById(_eula.Id, _eula.UserId);
    }

    private async Task HandleInvalidSubmit()
    {
    }
}