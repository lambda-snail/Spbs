namespace Spbs.Main.Core.Models;

public class Location
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public Location? Parent { get; set; }

    public List<Location> Children { get; set; } = new();

    public Location() { }
    
    public Location(string name, Location? parent = null)
    {
        Name = name;
        Parent = parent;
    }
}