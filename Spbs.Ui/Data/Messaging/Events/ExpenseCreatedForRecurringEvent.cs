using System;
using Microsoft.Azure.Cosmos;
#pragma warning disable CS8618

namespace Spbs.Ui.Data.Messaging.Events;

public class ExpenseInformation
{
    public Guid ExpenseId { get; set; }
    public Guid RecurringExpenseId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public double Total { get; set; }
}

public class ExpenseCreatedForRecurringEvent
{
    public string OriginatingSource { get; set; }
    public ExpenseInformation Expense { get; set; }
}