using AutoMapper;
using Integrations.Nordigen.Models;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Mapping;

public class InstituionMapper : Profile
{
    public InstituionMapper()
    {
        CreateMap<Aspsp, Institution>();
    }
}