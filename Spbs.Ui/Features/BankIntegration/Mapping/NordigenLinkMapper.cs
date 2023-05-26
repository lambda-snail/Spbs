using System;
using AutoMapper;
using Integrations.Nordigen.Models;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Mapping;

public class NordigenLinkMapper : Profile
{
    public NordigenLinkMapper()
    {
        CreateMap<NordigenLink, NordigenLink>()
            .ForMember(l => l.UserId, opt => opt.Condition(src => src.UserId != Guid.Empty));
            
        CreateMap<RequisitionV2, NordigenLink>()
            .ForMember(l => l.Id, c => c.MapFrom(x => x.Reference))
            .ForMember(l => l.NordigenId, c => c.MapFrom(x => x.Id))
            .ForMember(l => l.AuthorizationLink, c => c.MapFrom(x => x.Link));
    }
}