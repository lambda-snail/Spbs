using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Shared;

[AuthenticationTaskExtension]
public partial class MainLayout : LayoutComponentBase
{
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