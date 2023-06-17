using AutoMapper;
using Spbs.Ui.Features.Users.Mapping;

namespace Spbs.Ui.User.Test;

public class UsersMapperTests
{
    private readonly MapperConfiguration _configuration;
    private readonly IMapper _mapper;

    public UsersMapperTests()
    {
        _configuration = new MapperConfiguration(
            cfg => cfg.AddProfile(new UserMaps())
        );

        _mapper = _configuration.CreateMapper();
    }
    
    [Fact(DisplayName = "Test that the mapper configuration is valid")]
    public void MapperConfiguration_IsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }
}
