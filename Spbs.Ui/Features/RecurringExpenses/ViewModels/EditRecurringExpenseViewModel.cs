using System;
using System.ComponentModel.DataAnnotations;

namespace Spbs.Ui.Features.RecurringExpenses;

public class EditRecurringExpenseViewModel
{
    public Guid? Id { get; set; }
    public Guid? OwningUserId { get; set; }

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    
    [Required]
    public DateTime BillingDate { get; set; }
    [Required]
    public string BillingPrincipal { get; set; }
    [Required]
    public double Total { get; set; }
    [Required]
    public string Currency { get; set; }
    
    public string? Tags { get; set; }

    [Required]
    public BillingType BillingType { get; set; } = BillingType.Monthly;
    [Required]
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.Subscription;
}