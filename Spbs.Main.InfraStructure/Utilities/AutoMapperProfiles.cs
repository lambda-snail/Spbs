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

        CreateMap<PurchaseDto, Purchase>()
            .ForMember(purchase => purchase.Total, action => action.Ignore());

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        
        CreateMap<UserSettings, UserSettingsDto>()
            .ForMember(settingsDto => settingsDto.TimeZone, action => action.MapFrom(settings => settings.TimeZone.Id));
        
        CreateMap<UserSettingsDto, UserSettings>()
            .ForMember(
settings => settings.TimeZone, 
    action => 
                    action.MapFrom( settingsDto => TimeZoneInfo.FindSystemTimeZoneById(settingsDto.TimeZone)));
    }
}