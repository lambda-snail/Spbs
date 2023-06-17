using System;
using System.Globalization;
using AutoMapper;

namespace Spbs.Ui.Features.Users.Mapping;

public class UserMaps : Profile
{
    public UserMaps()
    {
        CreateMap<LocaleInformation, LocaleInformationViewModel>()
            .ForMember(
                livm => livm.TimeZone,
                opt => opt.MapFrom(li => li.TimeZone.Id))
            .ForMember(
            livm => livm.CultureInfo,
            opt => opt.MapFrom(li => li.CultureInfo.Name));
        
        CreateMap<LocaleInformationViewModel, LocaleInformation>()
            .ForMember(
                li => li.CultureInfo,
                opt => opt.MapFrom(livm => CultureInfo.CreateSpecificCulture(livm.CultureInfo)));
    }
}