using System;

namespace Spbs.Ui.Components.UserSettings;

/// <summary>
/// Allows components to specify how they should be represented in menus.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MenuNameAttribute : Attribute
{
    public string Name { get; }

    public MenuNameAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        Name = name;
    }
}