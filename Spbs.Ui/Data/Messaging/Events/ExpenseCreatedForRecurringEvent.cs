using System;

namespace Spbs.Ui.Data.Messaging.Events;

public class ExpenseCreatedForRecurringEvent
{
    public Guid ExpenseId { get; set; }
    public Guid RecurringExpenseId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public double Total { get; set; }
}