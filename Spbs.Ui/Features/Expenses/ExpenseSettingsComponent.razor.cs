using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Spbs.Ui.Components.UserSettings;

namespace Spbs.Ui.Features.Expenses;

[MenuName("Expenses")]
public partial class ExpenseSettingsComponent : UserSettingsComponentBase
{
#pragma warning disable CS8618
    [Inject] private IJSRuntime _jsRuntime { get; set; }
    
    private Grid<ExpenseCategoryListItem> _grid;
#pragma warning restore CS8618

    private HashSet<ExpenseCategoryListItem> _selectedCategories = new();
    
    private struct ExpenseCategoryListItem
    {
        public string CategoryName { get; set; }
        public static implicit operator ExpenseCategoryListItem(string str) => new ExpenseCategoryListItem() { CategoryName = str};
    }
    
    private List<ExpenseCategoryListItem> _expenseCategories = new();

    protected override void OnInitialized()
    {
        if (UserObject.ExpenseCategories is { Count: >0 })
        {
            _expenseCategories =
                UserObject.ExpenseCategories.Select(str =>
                    new ExpenseCategoryListItem { CategoryName = str }).ToList();
        }
    }

    private async Task OnAddCategoryClickedAsync()
    {
        string? newCategory = await _jsRuntime.InvokeAsync<string>("prompt", "Specify new category name");
        if (string.IsNullOrWhiteSpace(newCategory))
        {
            return;
        }

        if (_expenseCategories.Contains(newCategory))
        {
            return;
        }
        
        _expenseCategories.Add(newCategory);
        await _grid.RefreshDataAsync();
    }

    private Task OnSelectionChanged(HashSet<ExpenseCategoryListItem> categories)
    {
        if (categories is { Count: >0 })
        {
            _selectedCategories = categories;
        }

        return Task.CompletedTask;
    }
    
    private async Task OnRemoveCategoryClickedAsync()
    {
        foreach (var category in _selectedCategories)
        {
            _expenseCategories.Remove(category.CategoryName);
        }
        
        _selectedCategories.Clear();
        await _grid.RefreshDataAsync();
    }

    private void OnSaveClicked()
    {
        UserSettingsChangedCallback?.Invoke();
    }
}