using System.Text;
using Spbs.Main.Core.Models;
using Spbs.Main.InfraStructure.DtoModels;

namespace Spbs.Main.InfraStructure.Utilities;

/// <summary>
/// Maps between the Location hierarchy of the Location model and the Path field in the coresponding Dto.
/// </summary>
public static class LocationHelper
{
    /// <summary>
    /// Given a collection of LocationDto objects, this method will construct a collection of Location objects with populated
    /// parent and children fields. The collection is a dictionary to allow for efficient lookup of all locations.
    /// </summary>
    public static IDictionary<Guid, Location> CreateLocationContext(IEnumerable<LocationDto> context)
    {
        IDictionary<Guid, Location> locations = new Dictionary<Guid, Location>();

        foreach (var dto in context)
        {
            Location l = new Location()
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Name = dto.Name,
                Parent = null
            };
            
            locations.Add(l.Id, l);
        }

        foreach (var dto in context)
        {
            if (dto.ParentId is not null && locations.Keys.Contains((Guid)dto.ParentId!))
            {
                Guid parentId = (Guid)dto.ParentId!;
                Location location = locations[dto.Id];
                Location parent = locations[parentId];

                location.Parent = parent;
                parent.Children.Add(location);
            }
        }
        
        return locations;
    }
}