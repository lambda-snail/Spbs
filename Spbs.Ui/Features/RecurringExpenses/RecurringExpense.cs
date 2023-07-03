#nullable enable
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spbs.Data.Cosmos;

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
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// The related expense created for this history item
    /// </summary>
    [JsonProperty("expenseId")]
    public Guid ExpenseId { get; set; }
    [JsonProperty("total")]
    public double Total { get; set; }
    [JsonProperty("date")]
    public DateTime? Date { get; set; }
}

public class RecurringExpense : ICosmosData
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("userId")]
    public Guid UserId { get; set; }
    
    
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }
    [JsonProperty("billingDay")]
    public int BillingDay { get; set; }
    [JsonProperty("category")]
    public string? Category { get; set; }
    [JsonProperty("billingPrincipal")]
    public string BillingPrincipal { get; set; }
    [JsonProperty("total")]
    public double Total { get; set; }
    [JsonProperty("currency")]
    public string Currency { get; set; }
    [JsonProperty("tags")]
    public string? Tags { get; set; }
    
    [JsonProperty("paymentHistory")]
    public List<RecurringExpenseHistoryItem> PaymentHistory { get; set; } = new();
    
    [JsonProperty("billingType")]
    public BillingType BillingType { get; set; } = BillingType.Monthly;
    [JsonProperty("recurrenceType")]
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.Subscription;
    
    [JsonProperty("createdOn")]
    public DateTime CreatedOn { get; set; }
    [JsonProperty("modifiedOn")]
    public DateTime ModifiedOn { get; set; }
    
    public string GetDetailsUrl()
    {
        return $"recurring-expenses/{Id}";
    }

    /// <summary>
    /// Simple method that does not actually take into account holidays or week ends.
    /// <example>If BillingDay is 30 but is evaluated on february during a leap year, the actual billing day is 29.</example>
    /// </summary>
    public int GetActualBillingDay(int year, int month)
    {
        if (BillingDay <= 28) return BillingDay;
        var daysInMonth = DateTime.DaysInMonth(year, month);
        return BillingDay <= daysInMonth ? BillingDay : daysInMonth;
    }
}