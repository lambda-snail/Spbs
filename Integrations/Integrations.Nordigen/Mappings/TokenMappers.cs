using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Integrations.Nordigen.Models;

namespace Integrations.Nordigen.Mappings;

public class TokenMappers : Profile
{
    public TokenMappers()
    {
        CreateMap<SpectacularJWTObtain, TokenPair>()
            .ConstructUsing(jwt => new TokenPair(jwt.Access, jwt.Refresh, jwt.AccessExpires, jwt.RefreshExpires))
            .ForMember(dst => dst.Access, o => o.Ignore())
            .ForMember(dst => dst.AccessExpires, o => o.Ignore())
            .ForMember(dst => dst.Refresh, o => o.Ignore())
            .ForMember(dst => dst.RefreshExpires, o => o.Ignore());
        
        CreateMap<SpectacularJWTRefresh, TokenRefreshResult>()
            .ConstructUsing(jwt => new TokenRefreshResult(jwt.Access, jwt.AccessExpires))
            .ForMember(dst => dst.Access, o => o.Ignore())
            .ForMember(dst => dst.AccessExpires, o => o.Ignore());
    }
}