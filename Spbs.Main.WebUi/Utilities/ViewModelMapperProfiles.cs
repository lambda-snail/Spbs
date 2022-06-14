using AutoMapper;
using Spbs.Main.Core.Models;
using Spbs.Main.WebUi.ViewModels;

namespace Spbs.Main.WebUi.Utilities;

public class ViewModelMapperProfiles : Profile
{
    public ViewModelMapperProfiles()
    {
        CreateMap<NewPurchaseViewModel, Purchase>()
            .ForMember(p => p.Id, a => a.Ignore())
            .ForMember(p => p.OwnerId, a => a.Ignore())
            .ForMember(p => p.Items, a => a.Ignore())
            .ForMember(p => p.Total, action => action.Ignore());
        CreateMap<NewPurchaseItemViewModel, PurchaseItem>();
    }
}