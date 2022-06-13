using AutoMapper;
using Spbs.Main.InfraStructure.Utilities;
using Xunit;

namespace Sbps.Main.InfraStructure.Tests;

public class AutoMapperTests
{
    private MapperConfiguration GetMapperConfiguration()
    {
        return new MapperConfiguration(mapperconfig =>
        {
            mapperconfig.AddProfile(new AutoMapperProfiles());
        });
    }
    
    [Fact]
    public void TestAutoMapperProfiles()
    {
        var config = GetMapperConfiguration();
        config.AssertConfigurationIsValid();
    }
}