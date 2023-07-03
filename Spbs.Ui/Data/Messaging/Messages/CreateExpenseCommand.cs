using System;

namespace Spbs.Ui.Data.Messaging.Messages;

public class CreateExpenseCommand
{
    public CreateExpenseCommandPayload Expense { get; set; }
}

public class CreateExpenseCommandPayload
{
    public Guid UserId { get; set; }
    
    public string Name { get; set; }
    public string? Description { get; set; }
    
    public bool Recurring { get; set; } = false;
    public Guid RecurringExpenseId { get; set; }
    
    public DateTime Date { get; set; }
    public string? Venue { get; set; }
    public string? Category { get; set; }

    public double Total { get; set; }
    public string Currency { get; set; }
}