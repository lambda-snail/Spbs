using System;
using Newtonsoft.Json;

namespace Spbs.Ui.Features.Visualization.Models;

/// <summary>
/// A view of an expense that can be used in time series representations
/// </summary>
public class ExpenseSeriesVisualizationModel
{
    [JsonProperty("date")] 
    public DateTime Date { get; set; }

    // [JsonProperty("category")] 
    // public string? Category { get; set; }

    [JsonProperty("total")] 
    public decimal Total { get; set; }

    [JsonProperty("currency")] 
    public string Currency { get; set; }
}