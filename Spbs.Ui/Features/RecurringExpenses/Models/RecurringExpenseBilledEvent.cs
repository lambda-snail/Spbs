using System;

namespace Spbs.Ui.Features.RecurringExpenses.Models;

/// <summary>
/// Event published to the message broker to indicate that an expense should be generated from a given recurring expense.
/// </summary>
public record RecurringExpenseBilledEvent
{
    public Guid RecurringExpenseId { get; set; }
    public DateTime BillingDate { get; set;  }
}