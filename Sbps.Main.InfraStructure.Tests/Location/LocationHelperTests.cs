using System;
using System.Collections.Generic;
using AutoFixture;
using Spbs.Main.InfraStructure.Utilities;
using Spbs.Main.Core.Models;
using Spbs.Main.InfraStructure.DtoModels;
using Xunit;

namespace Sbps.Main.InfraStructure.Tests;

public class LocationHelperTests
{
    private readonly Fixture _fixture;

    public LocationHelperTests()
    {
        _fixture = new();
    }

    [Fact]
    public void CreateLocationContext_ShouldHandleEmptyInput()
    {
        var context = LocationHelper.CreateLocationContext(new List<LocationDto>());
        Assert.Empty(context);
    }

    [Fact]
    public void CreateLocationContext_ShouldHandleTree_DepthOf_1()
    {
        int numLocations = 10;
        IEnumerable<LocationDto> locationDtos =
            _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).CreateMany(numLocations);
        
        IDictionary<Guid, Location> locations = LocationHelper.CreateLocationContext(locationDtos);

        Assert.Equal(numLocations, locations.Count);
    }

    [Fact]
    public void CreateLocationContext_ShouldHandleTree_DepthOf_2()
    {
        //     1    4
        //    2  3
        var dto1 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();
        var dto2 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();
        var dto3 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();
        var dto4 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();

        dto2.ParentId = dto1.Id;
        dto3.ParentId = dto1.Id;
        
        List<LocationDto> context = new() { dto1, dto2, dto3, dto4 };
        IDictionary<Guid, Location> locations = LocationHelper.CreateLocationContext(context);

        var loc1 = locations[dto1.Id];
        var loc2 = locations[dto2.Id];
        var loc3 = locations[dto3.Id];
        var loc4 = locations[dto4.Id];
        
        Assert.Equal(loc1.Id, loc2.Parent!.Id);
        Assert.Equal(loc1.Id, loc3.Parent!.Id);
        Assert.Null(loc4.Parent);
        Assert.Contains(loc2, loc1.Children);
        Assert.Contains(loc3, loc1.Children);
        Assert.Empty(loc2.Children);
        Assert.Empty(loc3.Children);
        Assert.Empty(loc4.Children);
    }

    [Fact]
    public void CreateLocationContext_ShouldHandleTree_DepthOf_3()
    {
        var dto1 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();
        var dto2 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();
        var dto3 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();
        var dto4 = _fixture.Build<LocationDto>().With(dto => dto.ParentId, null as Guid?).Create();

        dto2.ParentId = dto1.Id;
        dto3.ParentId = dto1.Id;
        dto4.ParentId = dto2.Id;
        
        List<LocationDto> context = new() { dto1, dto2, dto3, dto4 };
        IDictionary<Guid, Location> locations = LocationHelper.CreateLocationContext(context);

        var loc1 = locations[dto1.Id];
        var loc2 = locations[dto2.Id];
        var loc3 = locations[dto3.Id];
        var loc4 = locations[dto4.Id];
        
        Assert.Equal(loc1.Id, loc2.Parent!.Id);
        Assert.Equal(loc1.Id, loc3.Parent!.Id);
        Assert.Equal(loc2.Id, loc4.Parent!.Id);
        Assert.Contains(loc4, loc2.Children);
        Assert.Contains(loc2, loc1.Children);
        Assert.Contains(loc3, loc1.Children);
        Assert.Empty(loc4.Children);
    }
}