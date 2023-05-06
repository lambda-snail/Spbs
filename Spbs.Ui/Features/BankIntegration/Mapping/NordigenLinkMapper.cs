using AutoMapper;
using Integrations.Nordigen.Models;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Mapping;

public class NordigenLinkMapper : Profile
{
    public NordigenLinkMapper()
    {
        CreateMap<RequisitionV2, NordigenLink>()
            .ForMember(l => l.Id, c => c.MapFrom(x => x.Reference))
            .ForMember(l => l.NordigenId, c => c.MapFrom(x => x.Id))
            .ForMember(l => l.AuthorizationLink, c => c.MapFrom(x => x.Link));
    }
}