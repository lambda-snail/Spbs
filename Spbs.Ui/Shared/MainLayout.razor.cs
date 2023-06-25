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

    private async Task<SidebarDataProviderResult> SidebarDataProvider(SidebarDataProviderRequest request)
    {
        if (_navItems is null)
            _navItems = GetNavItems();

        return await Task.FromResult(request.ApplyTo(_navItems));
    }

    private IEnumerable<NavItem> GetNavItems()
    {
        _navItems = new List<NavItem>
        {
            new NavItem { Id = "1", Href = "/home", IconName = IconName.HouseDoorFill, Text = "Home" },
            new NavItem { Id = "2", Href = "/expenses", IconName = IconName.CurrencyEuro, Text = "Expenses", IconColor = IconColor.Primary },
            new NavItem { Id = "3", Href = "/recurring", IconName = IconName.Calendar, Text = "Recurring Expenses", IconColor = IconColor.Primary },
            new NavItem { Id = "4", Href = "/data/visualize", IconName = IconName.PieChartFill, Text = "Visualize", IconColor = IconColor.Primary },
            new NavItem { Id = "5", IconName = IconName.PiggyBankFill, Text = "Bank Integration", IconColor = IconColor.Success },
            new NavItem { Id = "6", Href = "/accounts/links/", IconName = IconName.GearWideConnected, Text = "Import Expenses", ParentId = "5" },
            new NavItem { Id = "7", Href = "/external/banks", IconName = IconName.BookmarkPlus, Text = "Link to Bank", ParentId = "5" },
        };

        return _navItems;
    }

    private void ToggleSidebar() => _sidebar.ToggleSidebar();
}