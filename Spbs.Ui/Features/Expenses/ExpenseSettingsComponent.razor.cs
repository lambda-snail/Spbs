using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Ui.Components.UserSettings;

namespace Spbs.Ui.Features.Expenses;

[MenuName("Expenses")]
public partial class ExpenseSettingsComponent : UserSettingsComponentBase
{
#pragma warning disable CS8618
    [Inject] private ISnackbar _snackbar { get; set; }
    private MudDataGrid<ExpenseCategoryListItem> _grid;
#pragma warning restore CS8618

    private HashSet<ExpenseCategoryListItem> _selectedCategories = new();
    private bool _isDirty = false;
    
#pragma warning disable CS8618
    private class ExpenseCategoryListItem
    {
        public string CategoryName { get; set; }
        public static implicit operator ExpenseCategoryListItem(string str) => new ExpenseCategoryListItem() { CategoryName = str };
    }
#pragma warning restore CS8618
    
    private List<ExpenseCategoryListItem> _expenseCategories = new();

    protected override void OnInitialized()
    {
        if (UserObject.ExpenseCategories is null)
        {
            UserObject.ExpenseCategories = new();
        }
        
        if (UserObject.ExpenseCategories is { Count: >0 })
        {
            _expenseCategories =
                UserObject.ExpenseCategories.Select(str =>
                    new ExpenseCategoryListItem { CategoryName = str }).ToList();
        }
    }

    private async Task OnAddCategoryClickedAsync()
    {
        var newCategory = new ExpenseCategoryListItem { CategoryName = "new category" };
        _expenseCategories.Add(newCategory);
        await _grid.ReloadServerData();
        await _grid.SetEditingItemAsync(newCategory);
        _isDirty = true;
    }
    
    private async Task OnCommittedItemChanged(ExpenseCategoryListItem category)
    {
        if (string.IsNullOrWhiteSpace(category.CategoryName))
        {
            _expenseCategories.Remove(category);
            _snackbar.Add("Cannot save an empty category", Severity.Error);
            return;
        }

        var duplicates = _expenseCategories.Count(c => c.CategoryName == category.CategoryName);
        if (duplicates > 1)
        {
            _expenseCategories.Remove(category);
            _snackbar.Add("Category already exists", Severity.Warning);
            return;
        }
        
        await _grid.ReloadServerData();
        _snackbar.Add("Category added!", Severity.Success);
        _isDirty = true;
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
            _expenseCategories.Remove(category);
        }
        
        _selectedCategories.Clear();
        await _grid.ReloadServerData();
        _snackbar.Add("Category removed", Severity.Success);
        _isDirty = true;
    }
    
    private async Task EditSelectedItem()
    {
        var selectedItems = _grid.SelectedItems;
        if (selectedItems is { Count: not 1 })
        {
            _snackbar.Add("Too many items selected", Severity.Warning);
        }

        await  _grid.SetEditingItemAsync(selectedItems.First());
    }

    private void OnSaveClicked()
    {
        if (!_isDirty)
        {
            return;
        }
        
        UserObject.ExpenseCategories = _expenseCategories.Select(c => c.CategoryName).ToList();
        UserSettingsChangedCallback?.Invoke();
    }

    private string DeleteButtonTooltip()
    {
        if (_selectedCategories is { Count: 0 })
        {
            return "Delete category (none selected)";
        }

        return "Delete category";
    }

    private string EditButtonTooltip()
    {
        return _selectedCategories switch
        {
            { Count: 0 } => "Edit category (none selected)",
            { Count: >1 } => "Edit category (too many selected)",
            _ => "Edit category"
        };
    }

    private string SaveButtonTooltip()
    {
        return _isDirty ? "Save changes" : "No changes to save";
    }
}