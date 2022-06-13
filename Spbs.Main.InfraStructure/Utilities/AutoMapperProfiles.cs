using AutoMapper;
using Spbs.Main.Core.Models;
using Spbs.Main.InfraStructure.DtoModels;

namespace Spbs.Main.InfraStructure.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<PurchaseItem, PurchaseItemDto>();
        CreateMap<PurchaseItemDto, PurchaseItem>();

        CreateMap<Purchase, PurchaseDto>()
            .ForMember(pdto => pdto.ModelVersion, action => action.Ignore());
        
        CreateMap<PurchaseDto, Purchase>();
    }
}