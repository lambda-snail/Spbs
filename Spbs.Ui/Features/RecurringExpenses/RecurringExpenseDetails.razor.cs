#nullable enable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Auth;

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
    
    [Inject] public IRecurringExpenseReaderRepository _expenseReaderRepository { get; set; }

    private RecurringExpense? _expense;

    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        if (userId is null)
        {
            return;
        }

        _expense = (_expenseId is not null) ? await _expenseReaderRepository.GetByIdAsync(userId.Value, _expenseId.Value) : new();
        StateHasChanged();
    }
}