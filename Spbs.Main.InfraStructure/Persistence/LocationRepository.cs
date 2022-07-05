using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Settings;
using Spbs.Main.InfraStructure.DtoModels;
using Spbs.Main.InfraStructure.Utilities;

namespace Spbs.Main.InfraStructure.Persistence;

public class LocationRepository : ILocationRepository
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<LocationDto> _locations;
    
    public LocationRepository(IMongoClient mongoCLient, IMapper mapper, IOptions<SpbsDatabaseSettings> options)
    {
        var client = mongoCLient.GetDatabase(options.Value.DatabaseName);
        _locations = client.GetCollection<LocationDto>(options.Value.LocationsCollection);
        _mapper = mapper;
    }
    
    public async Task<IDictionary<Guid, Location>> GetAllLocations(Guid userId)
    {
        var filter = Builders<LocationDto>.Filter.Where(dto => dto.UserId == userId);
        var result = await _locations.FindAsync(filter);
        var locationDtos = await result.ToListAsync();
        return LocationHelper.CreateLocationContext(locationDtos);
    }

    public async Task InsertSingleLocation(Location location)
    {
        // We need to update the location and its children - if any
        LocationDto locationDto = _mapper.Map<LocationDto>(location);
        List<LocationDto> locationsUpdateBuffer = _mapper.Map<List<LocationDto>>(location.Children);
        locationsUpdateBuffer.Add(locationDto);
            
        await _locations.InsertManyAsync(locationsUpdateBuffer);
    }

    public Task UpdateSingleLocation(Location location)
    {
        throw new NotImplementedException();
    }
}