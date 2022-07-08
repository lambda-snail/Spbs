using Spbs.Main.Core.Models;

namespace Spbs.Main.InfraStructure.DtoModels;

public interface ILocationContext
{
    /// <summary>
    /// Loads all locations of the currently logged in user into memory.
    /// </summary>
    Task InitLocations();
    public bool IsInitialized { get; }
    void AddLocation(Location location);
    void RemoveLocation(Location location, RemoveLocationMode mode);

    /// <summary>
    /// Returns a collection of all locations that do not have a parent.
    /// </summary>
    IEnumerable<Location> GetTopLevelLocations();

    /// <summary>
    /// Returns a collection of all locations that do not have any children.
    /// </summary>
    IEnumerable<Location> GetBottomLevelLocations();

    /// <summary>
    /// Get all bottom-level locations that have a given ancestor. 
    /// </summary>
    IEnumerable<Location> GetBottomLevelLocationsByAncestor(Location location);

    /// <summary>
    /// Retreieve a location from the context by id, or null if the location does not exist.
    /// </summary>
    Location? GetLocation(Guid id);

    bool Empty { get; }
    bool Contains(Location location);
}