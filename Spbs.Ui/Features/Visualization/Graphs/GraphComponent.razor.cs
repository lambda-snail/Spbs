using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Spbs.Ui.Features.Visualization.Graphs;

public partial class GraphComponent<TDataItem> : ComponentBase
{
    private readonly string _canvasId = "chartjs-canvas-" + DateTime.Now;

#pragma warning disable CS8618
    [Inject] IJSRuntime _jSRuntime { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// The items from which to create the data visualization.
    /// </summary>
    [Parameter] public IReadOnlyList<TDataItem> DataItems { get; set; } = new List<TDataItem>();

    /// <summary>
    /// A function that tells the grap  component how to extract values to be displayed in the graph
    /// from a TDataItem.
    /// </summary>
    [Parameter] public Func<TDataItem, double> ToValueTransform { get; set; }

    /// <summary>
    /// Given a TDataItem, what label should be displayed in the graph?
    /// </summary>
    [Parameter] public Func<TDataItem, string> ToLabelTransform { get; set; }
    
    private List<string> _filteredLabels = new();
    private List<double> _filteredValues = new();
    private const string _noCategoryLabel = "Unassigned";
    
    private IJSObjectReference _chart;
    private bool _isChartCreated = false;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isChartCreated) return;

        SumByCategory();
        
        var config = new
        {
            Type = "bar",
            Options = new
            {
                Responsive = true,
                Scales = new
                {
                    Y = new
                    {
                        BeginAtZero = true
                    }
                }
            },
            Data = new
            {
                Labels = _filteredLabels,
                Datasets = new[]
                {
                    new
                    {
                        Label = "Expenses per month",
                        Data = _filteredValues,
                    }
                }
            }
        };
        
        _chart = await _jSRuntime!.InvokeAsync<IJSObjectReference>("createChart", _canvasId, config);
        _isChartCreated = true;
    }
    
    private void SumByCategory()
    {
        Dictionary<string, double> graphData = new(DataItems.Count);

        foreach (var item in DataItems)
        {
            double value = ToValueTransform(item);
            string label = ToLabelTransform(item);
            
            if (string.IsNullOrWhiteSpace(label))
            {
                label = _noCategoryLabel;
            }
                
            if(graphData.ContainsKey(label))
            {
                graphData[label] += value;
            }
            else
            {
                graphData[label] = value;
            }
        }

        _filteredLabels = new(graphData.Keys.Count);
        _filteredValues = new(graphData.Keys.Count);
        foreach (var data in graphData)
        {
            _filteredLabels.Add(data.Key);
            _filteredValues.Add(data.Value);
        }
    }

}