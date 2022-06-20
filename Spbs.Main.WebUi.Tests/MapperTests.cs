using AutoMapper;
using Spbs.Main.WebUi.Utilities;
using Xunit; 

namespace Spbs.Main.WebUi.Tests;

public class MapperTests
{
    private MapperConfiguration GetMapperConfiguration()
    {
        return new MapperConfiguration(mapperconfig =>
        {
            mapperconfig.AddProfile(new ViewModelMapperProfiles());
        });
    }
    
    [Fact]
    public void MapperConfiguration_ShouldBeValid()
    {
        var configuration = GetMapperConfiguration();
        configuration.AssertConfigurationIsValid();
    }
}