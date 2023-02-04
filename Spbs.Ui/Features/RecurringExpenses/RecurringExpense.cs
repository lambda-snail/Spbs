#nullable enable
using System;
using System.Collections.Generic;
using System.Security.Principal;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.RecurringExpenses;

public enum BillingType
{
    Monthly,
    Yearly
}

public enum RecurrenceType
{
    Bill,
    Subscription,
    Invoice,
    Mortgage
}

public class RecurringExpenseHistoryItem
{
    public Guid Id { get; set; }
    public double Total { get; set; }
    public DateTime Date { get; set; }
    public bool Payed { get; set; }
}

public class RecurringExpense
{
    public Guid Id { get; set; }
    public Guid OwningUserId { get; set; }
    
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime BillingDate { get; set; }
    public string BillingPrincipal { get; set; }
    public double Total { get; set; }
    public string? Tags { get; set; }
    
    public List<RecurringExpenseHistoryItem> PaymentHistory { get; set; } = new();
    
    public BillingType BillingType { get; set; } = BillingType.Monthly;
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.Subscription;
    
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}