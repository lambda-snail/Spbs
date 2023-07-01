using System;
using System.Collections.Generic;
using Spbs.Data.Cosmos;

namespace Spbs.Ui.Features.Expenses.Models;

/// <summary>
/// A subset of the full user to fetch only what is needed by the expense feature.
/// </summary>
public class ExpenseUser : ICosmosData
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public List<string> ExpenseCategories { get; set; } = new();
}
