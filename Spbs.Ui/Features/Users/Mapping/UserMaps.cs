using System;
using AutoMapper;

namespace Spbs.Ui.Features.Users.Mapping;

public class UserMaps : Profile
{
    public UserMaps()
    {
        CreateMap<LocaleInformation, LocaleInformationViewModel>()
            .ForMember(
                livm => livm.TimeZone,
                opt => opt.MapFrom(li => li.TimeZone.Id));

        CreateMap<LocaleInformationViewModel, LocaleInformation>()
            .ForMember(
                li => li.TimeZone,
                opt => opt.MapFrom(livm => TimeZoneInfo.FindSystemTimeZoneById(livm.TimeZone)));
    }
}