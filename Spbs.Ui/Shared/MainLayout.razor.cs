using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Shared;

[AuthenticationTaskExtension]
public partial class MainLayout : LayoutComponentBase
{
    IEnumerable<NavItem>? _navItems;

#pragma warning disable CS8618
    Sidebar _sidebar;
#pragma warning restore CS8618
    
    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        ArgumentNullException.ThrowIfNull(userId);
    }
    
    bool _drawerOpen = true;
    private bool _isDarkMode = true;
    
    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}