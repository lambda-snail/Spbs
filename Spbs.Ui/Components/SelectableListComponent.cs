using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Components;

/// <summary>
/// A component that has a list of T, and is able to select an item from that list.
/// </summary>
public abstract class SelectableListComponent<T> : ComponentBase
{
    /// <summary>
    /// This method is used by the selection logic to get hold of an instance of the list.
    /// </summary>
    protected abstract List<T>? GetList();

    private int? _selectedRow;
    
    /// <summary>
    /// Determine which row is the selected one. If the selected row is clicked twice, it will be deselected.
    /// </summary>
    public void SetSelected(int i)
    {
        if (i >= 0 && i < GetList()?.Count)
        {
            _selectedRow = i == _selectedRow ? null : i;
            StateHasChanged();
        }
        else
        {
            if (_selectedRow is not null)
            {
                _selectedRow = null;
                StateHasChanged();
            }
        }
    }

    public int? GetSelected() => _selectedRow;

    public bool IsSelected(int i) => _selectedRow == i;
    
    /// <summary>
    /// Clears the selected index. Returns the value that was selected, if any.
    /// </summary>
    /// <returns></returns>
    public int? ClearSelection()
    {
        int? selected = _selectedRow;
        _selectedRow = null;
        return selected;
    }
}