using System;
using System.Collections.Generic;

namespace Spbs.Ui.Features.Expenses;

public class Expense
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public bool Recurring { get; set; } = false;

    public Guid OwningUserId { get; set; }

    public DateTime Date { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string? Venue { get; set; }

//    public Category? Category { get; set; }

    public List<ExpenseItem> Items { get; set; } = new();
    public string? Tags { get; set; }

    public double Total {
        get
        {
          double total = 0.0;
          for (int i = 0; i < Items.Count; ++i)
          {
              total += Items[i].GetCostOfItem();
          }
          return total;
        }
    }
    public string Currency { get; set; }
}

public class ExpenseItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public double GetCostOfItem()
    {
        return Price * Quantity;
    }
}