using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.Visualization.Models;

/// <summary>
/// A model for visualizaing expenses. Contains a subset of everything that the <see cref="Spbs.Ui.Features.Expenses">Expense</see>
/// contains. Not intended to be used for persistence.
/// </summary>
public class ExpenseVisualizationModel : ICosmosData
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("userId")]
    public Guid UserId { get; set; }

    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("venue")]
    public string? Venue { get; set; }

    [JsonProperty("category")]
    public string? Category { get; set; }

    [JsonProperty("items")]
    public List<ExpenseItem> Items { get; set; } = new();
    
    [JsonProperty("tags")]
    public string? Tags { get; set; }
    
    [JsonProperty("total")]
    public decimal Total { get; set; }
    
    [JsonProperty("currency")]
    public string Currency { get; set; }
}