using AutoMapper;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Components;

namespace Spbs.Ui.Features.Users;

public partial class UserLocaleSettingsComponent : UserSettingsComponentBase
{
    private LocaleInformationViewModel _localeInformationVm { get; set; } = new();

    [Inject] private IMapper _mapper { get; set; }

    protected override void OnInitialized()
    {
        _localeInformationVm = _mapper.Map<LocaleInformationViewModel>(UserObject.LocaleInformation);
    }

    private void HandleValidSubmit()
    {
        UserObject.LocaleInformation = _mapper.Map<LocaleInformation>(_localeInformationVm);
        UserSettingsChangedCallback?.Invoke();
    }

    private void HandleInvalidSubmit()
    {
        
    }
}