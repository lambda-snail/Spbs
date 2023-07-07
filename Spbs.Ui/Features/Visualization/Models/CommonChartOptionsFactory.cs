using ApexCharts;

namespace Spbs.Ui.Features.Visualization.Models;

public class CommonChartOptionsFactory
{
    private static string _textColor = "#FFFFFF";
    
    public static ApexChartOptions<TModel> CreateOptions<TModel>() where TModel : class
    {
        return new()
        {
            DataLabels = new()
            {
                Enabled = true,
                Style = new()
                {
                    Colors = new() { _textColor }
                }
            },
            PlotOptions = new()
            {
                Bar = new()
                {
                    BorderRadius = 4,
                    Horizontal = true,
                    DataLabels = new()
                    {
                        Position = "top",
                        Total = new()
                        {
                            Enabled = true,
                            Style = new()
                            {
                                Color = _textColor
                            }
                        }
                    }
                },
                Pie = new()
                {
                    Donut = new()
                    {
                        Labels = new()
                        {
                            Name = new()
                            {
                                Color = _textColor
                            }
                        }
                    }
                }
            },
            Legend = new()
            {
                Show = true,
                Labels = new()
                {
                    Colors = new Color(_textColor)
                }
            },
            Title = new()
            {
                Style = new()
                {
                    Color = _textColor
                }
            },
            Xaxis = new()
            {
                Labels = new()
                {
                    Style = new()
                    {
                        Colors = new Color(_textColor)
                    }
                }
            },
            Yaxis = new()
            {
                new()
                {
                    Labels = new()
                    {
                        Style = new()
                        {
                            Colors = new Color(_textColor)
                        }
                    }
                }
            },
            Chart = new()
            {
                Toolbar = new()
                {
                    Show = false
                }
            }
        };
    }
}