using AutoMapper;
using Integrations.Nordigen.Models;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Mapping;

public class EulaMapper : Profile 
{
    public EulaMapper()
    {
        CreateMap<EndUserAgreement, NordigenEula>();
    }
}