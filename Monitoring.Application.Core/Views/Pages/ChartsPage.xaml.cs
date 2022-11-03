using System;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;

namespace SystemMonitoringNetCore.Views.Pages;

public partial class ChartsPage : Page
{
    private PlotModel Model { get; set; }
    
    public ChartsPage()
    {
        InitializeComponent();

        Model = new PlotModel { Title = "Example Title" };
        Model.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
        OxyPlotView.Model = Model;
    }
}