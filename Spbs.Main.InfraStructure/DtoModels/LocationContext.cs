using Spbs.Main.Core.Models;

namespace Spbs.Main.InfraStructure.DtoModels;

/// <summary>
/// A class that represents the context of all locations for a specified user.
/// </summary>
public class LocationContext
{
    private readonly Dictionary<Guid, Location> _context;

    public LocationContext()
    {
        _context = new();
    }

    public LocationContext(Dictionary<Guid, Location> context)
    {
        _context = context;
    }
    
    public void AddLocation(Location location)
    {
        
    }

    public void RemoveLocation(Location location, RemoveLocationMode mode)
    {
        
    }

    /// <summary>
    /// Returns a collection of all locations that do not have a parent.
    /// </summary>
    public IEnumerable<Location> GetTopLevelLocations()
    {
        return _context.Values.Where(l => l.Parent is null);
    }

    /// <summary>
    /// Returns a collection of all locations that do not have any children.
    /// </summary>
    public IEnumerable<Location> GetBottomLevelLocations()
    {
        return _context.Values.Where(l => l.Children.Count == 0);
    }

    /// <summary>
    /// Get all bottom-level locations that have a given ancestor. 
    /// </summary>
    public IEnumerable<Location> GetBottomLevelLocationsByAncestor(Location location)
    {
        List<Location> locations = new();
        CollectLeafChildren(location, locations);
        return locations;
    }

    private void CollectLeafChildren(Location location, List<Location> collection)
    {
        if (location.Children.Count == 0)
        {
            collection.Add(location);
            return;
        }

        foreach (var child in location.Children)
        {
            CollectLeafChildren(child, collection);
        }
    }

    /// <summary>
    /// Retreieve a location from the context by id, or null if the location does not exist.
    /// </summary>
    public Location? GetLocation(Guid id)
    {
        return _context.ContainsKey(id) ? _context[id] : null;
    }

    public bool Empty => _context.Count == 0;
    public bool Contains(Location location) => _context.ContainsKey(location.Id);
}

public enum RemoveLocationMode
{
    /// <summary>
    /// When a location is removed, all sublocations will also be removed.
    /// </summary>
    RemoveSubtree,
    
    /// <summary>
    /// When a location is removed, its sublocations are relinked so that the location's parent becomes the new parent
    /// of the sublocations. 
    /// </summary>
    ReLinkSubtree
}