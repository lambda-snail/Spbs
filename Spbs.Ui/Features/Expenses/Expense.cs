using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spbs.Ui.Data.Cosmos;

namespace Spbs.Ui.Features.Expenses;

public class Expense : ICosmosData
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("recurring")]
    public bool Recurring { get; set; } = false;

    [JsonProperty("userId")]
    public Guid UserId { get; set; }

    [JsonProperty("date")]
    public DateTime Date { get; set; }
    [JsonProperty("createdOn")]
    public DateTime CreatedOn { get; set; }
    [JsonProperty("modifiedOn")]
    public DateTime ModifiedOn { get; set; }

    [JsonProperty("venue")]
    public string? Venue { get; set; }

    [JsonProperty("category")]
    public string? Category { get; set; }

    [JsonProperty("items")]
    public List<ExpenseItem> Items { get; set; } = new();
    [JsonProperty("tags")]
    public string? Tags { get; set; }

    public double? _total;
    
    [JsonProperty("total")]
    public double Total {
        get
        {
            if (_total is not null)
            {
                return _total.Value;
            }

            double total = 0.0;
            for (int i = 0; i < Items.Count; ++i)
            {
                total += Items[i].GetCostOfItem();
            }
            return total;
        }
        set => _total = value;
    }
    [JsonProperty("currency")]
    public string Currency { get; set; }
}

public class ExpenseItem
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
    [JsonProperty("price")]
    public double Price { get; set; }

    public double GetCostOfItem()
    {
        return Price * Quantity;
    }
}
