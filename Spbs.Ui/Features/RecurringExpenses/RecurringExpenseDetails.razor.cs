#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Utilities;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;
using Spbs.Ui.Data.Messaging.Commands;
using Spbs.Ui.Features.RecurringExpenses.Messaging;

namespace Spbs.Ui.Features.RecurringExpenses;

[AuthenticationTaskExtension()]
public partial class RecurringExpenseDetails
{
    private Guid? _expenseId;
    [Parameter]
    public string? ExpenseId
    {
        set => _expenseId = (value is null) ? null : Guid.Parse(value);
    }
    
#pragma warning disable CS8618
    [Inject] private IRecurringExpenseReaderRepository _expenseReader { get; set; }
    [Inject] private IRecurringExpenseWriterRepository _expenseWriter { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    
    [Inject] private RecurringExpenses_CreateExpenseCommandPublisher _publisher { get; set; }
    [Inject] private IMapper _mapper { get; set; }

    private EditRecurringExpenseComponent _expenseEditor;
    private MudDataGrid<RecurringExpenseHistoryItem> _grid;
    private int _numSelectedHistoryItems = 0;
#pragma warning restore CS8618
    
    private RecurringExpense? _expense;

    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        if (userId is null)
        {
            return;
        }

        _expense = (_expenseId is not null) ? await _expenseReader.GetByIdAsync(userId.Value, _expenseId.Value) : new();
        StateHasChanged();
    }

    private Task OnHistoryItemChanged(RecurringExpenseHistoryItem obj)
    {
        return _expenseWriter.UpsertExpenseAsync(_expense!);
    }

    private async Task AddExpenseItem()
    {
        var newItem = new RecurringExpenseHistoryItem { Id = Guid.NewGuid() };
        
        _expense!.PaymentHistory.Add(newItem);
        await _grid.ReloadServerData();
        await _grid.SetEditingItemAsync(newItem);
    }

    private async Task EditSelectedItem()
    {
        var selectedItems = _grid.SelectedItems;
        if (selectedItems is { Count: not 1 })
        {
            _snackbar.Add("Too many items selected", Severity.Warning);
        }

        await _grid.SetEditingItemAsync(selectedItems.First());
    }

    private async Task DeleteExpenseItems()
    {
        var selectedItems = _grid.SelectedItems;
        if (selectedItems is { Count: 0 })
        {
            _snackbar.Add("No items selected", Severity.Warning);
        }

        foreach (var item in selectedItems)
        {
            _expense!.PaymentHistory.Remove(item);
        }
        
        selectedItems.Clear();
        _numSelectedHistoryItems = 0;
        
        await _expenseWriter.UpsertExpenseAsync(_expense!);
        _snackbar.Add("History item(s) successfully deleted", Severity.Success);
    }

    private void OnSelectedItemsChanged(HashSet<RecurringExpenseHistoryItem> obj)
    {
        _numSelectedHistoryItems = _grid.SelectedItems.Count;
    }
    
    private string DeleteButtonTooltip()
    {
        if (_numSelectedHistoryItems == 0 )
        {
            return "Delete item (none selected)";
        }

        return "Delete item";
    }

    private string EditButtonTooltip()
    {
        return _numSelectedHistoryItems switch
        {
            0 => "Edit item (none selected)",
            >1 => "Edit item (too many selected)",
            _ => "Edit item"
        };
    }

    private Task RecurringExpenseUpdated(RecurringExpense expense)
    {
        _expense = expense;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private void ToggleEditMode()
    {
        _expenseEditor.SetModalContent(_expense);
        _expenseEditor.ShowModal();
    }

    private async Task CreateExpenseFromRecurring()
    {
        bool? shouldProceed = await DialogService.ShowMessageBox(
            "Create Expense", 
            "This will create a new expense based on the recurring expense.", 
            yesText:"Proceed", cancelText:"Cancel");
        
        if (shouldProceed is null)
        {
            return;
        }

        var options = new DialogOptions
        {
            CloseButton = true,
            DisableBackdropClick = false,
            MaxWidth = MaxWidth.Small
        };

        var parameters = new DialogParameters();
        parameters.Add("Title", "Choose Expense Date");
        //parameters.Add("Prompt", "What billing date would you like to set for the new expense?");
        
        var dialog = await DialogService.ShowAsync<InputDateDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;
        var input = result.Data as DateTime?;
        if (input is null)
        {
            return; // User cancelled
        }
        
        var expensePayload = _mapper.Map<CreateExpenseCommandPayload>(_expense);
        expensePayload.Date = input.Value.ToUniversalTime(); 
        await _publisher.PublishMessage(
            new CreateExpenseCommand()
            {
                Expense = expensePayload
            },
            RecurringExpenseMessagingConstants.CreateSingleExpense);
        
        await DialogService.ShowMessageBox(
            "Create Expense", 
            "A create operation has been queued and will be performed shortly", 
            yesText:"Got it!");
    }
}