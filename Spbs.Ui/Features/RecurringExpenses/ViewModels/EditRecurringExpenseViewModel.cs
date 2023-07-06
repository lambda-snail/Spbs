using System;

#pragma warning disable CS8618

namespace Spbs.Ui.Features.RecurringExpenses;

public class EditRecurringExpenseViewModel
{
    public Guid? Id { get; set; }
    public Guid? UserId { get; set; }
    
    public string Name { get; set; }
    public string? Description { get; set; }

    public string? Category { get; set; }
    
    public int BillingDay { get; set; }
    public string BillingPrincipal { get; set; }
    public double Total { get; set; }
    public string Currency { get; set; }
    
    public string? Tags { get; set; }
    
    public BillingType BillingType { get; set; } = BillingType.Monthly;
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.Subscription;
}