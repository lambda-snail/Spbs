using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Contracts;

public interface ILocationRepository
{
    Task<IDictionary<Guid, Location>> GetAllLocations(Guid userId);
    Task InsertSingleLocation(Location location);
    Task UpdateSingleLocation(Location location);
    //Task UpdateLocationContext(IDictionary<Guid, Location> context);
}