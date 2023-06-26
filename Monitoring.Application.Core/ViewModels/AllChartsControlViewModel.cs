using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Monitoring.Models.Entity;
using SkiaSharp;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class AllChartsControlViewModel : BaseViewModel
{
    #region Charts : List<ChartViewModel> - Список графиков

    private List<ChartViewModel> _charts;

    /// <summary> Список графиков </summary>
    public List<ChartViewModel> Charts
    {
        get => _charts;
        set => SetField(ref _charts, value);
    }

    #endregion Charts

    #region SelectedChart : ChartViewModel - Выбранный график

    private ChartViewModel _selectedChart;

    /// <summary> Выбранный график </summary>
    public ChartViewModel SelectedChart
    {
        get => _selectedChart;
        set => SetField(ref _selectedChart, value);
    }

    #endregion SelectedChart

    public AllChartsControlViewModel(List<SensorData> sensors, CultureStatus actualStatus)
    {
        Charts = new List<ChartViewModel>();

        Charts.Add(new ChartViewModel("Температура",
            new ISeries[] { new LineSeries<double> { Name = "Температура", Values = sensors.Select(x => x.Temperature).ToList(), Fill = null } },
            new[] { new RectangularSection { Yi = actualStatus.StartingValueTemperature, Yj = actualStatus.EndingValueTemperature, Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(60) } } },
            new List<ICartesianAxis> { new Axis { LabelsRotation = 15, UnitWidth = TimeSpan.FromDays(1).Ticks, ShowSeparatorLines = true, MinStep = 1, } },
            new List<ICartesianAxis> { new Axis { UnitWidth = 0, ShowSeparatorLines = true, MinStep = 1, } }
        ));
        Charts.Add(new ChartViewModel("Влажность",
            new ISeries[] { new LineSeries<double> { Name = "Влажность", Values = sensors.Select(x => x.Humidity).ToList(), Fill = null } },
            new[] { new RectangularSection { Yi = actualStatus.StartingValueHumidity, Yj = actualStatus.EndingValueHumidity, Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(60) } } },
            new List<ICartesianAxis> { new Axis { LabelsRotation = 15, UnitWidth = TimeSpan.FromDays(1).Ticks, ShowSeparatorLines = true, MinStep = 1, } },
            new List<ICartesianAxis> { new Axis { UnitWidth = 0, ShowSeparatorLines = true, MinStep = 1, } }
        ));
        Charts.Add(new ChartViewModel("Кислотность",
            new ISeries[] { new LineSeries<double> { Name = "Кислотность", Values = sensors.Select(x => x.Acidity).ToList(), Fill = null } },
            new[] { new RectangularSection { Yi = actualStatus.StartingValuePh, Yj = actualStatus.EndingValuePh, Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(60) } } },
            new List<ICartesianAxis> { new Axis { LabelsRotation = 15, UnitWidth = TimeSpan.FromDays(1).Ticks, ShowSeparatorLines = true, MinStep = 1, } },
            new List<ICartesianAxis> { new Axis { UnitWidth = 0, ShowSeparatorLines = true, MinStep = 1, } }
        ));
        Charts.Add(new ChartViewModel("Фосфор",
            new ISeries[] { new LineSeries<double> { Name = "Фосфор", Values = sensors.Select(x => x.Phosphorus).ToList(), Fill = null } },
            new[] { new RectangularSection { Yi = actualStatus.StartingValuePhosphor, Yj = actualStatus.EndingValuePhosphor, Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(60) } } },
            new List<ICartesianAxis> { new Axis { LabelsRotation = 15, UnitWidth = TimeSpan.FromDays(1).Ticks, ShowSeparatorLines = true, MinStep = 1, } },
            new List<ICartesianAxis> { new Axis { UnitWidth = 0, ShowSeparatorLines = true, MinStep = 1, } }
        ));
        Charts.Add(new ChartViewModel("Засоленность",
            new ISeries[] { new LineSeries<double> { Name = "Засоленность", Values = sensors.Select(x => x.Salinity).ToList(), Fill = null } },
            new[] { new RectangularSection { Yi = actualStatus.StartingValueSalinity, Yj = actualStatus.EndingValueSalinity, Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(60) } } },
            new List<ICartesianAxis> { new Axis { LabelsRotation = 15, UnitWidth = TimeSpan.FromDays(1).Ticks, ShowSeparatorLines = true, MinStep = 1, } },
            new List<ICartesianAxis> { new Axis { UnitWidth = 0, ShowSeparatorLines = true, MinStep = 1, } }
        ));
        Charts.Add(new ChartViewModel("Калий",
            new ISeries[] { new LineSeries<double> { Name = "Калий", Values = sensors.Select(x => x.Potassium).ToList(), Fill = null } },
            new[] { new RectangularSection { Yi = actualStatus.StartingValuePotassium, Yj = actualStatus.EndingValuePotassium, Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(60) } } },
            new List<ICartesianAxis> { new Axis { LabelsRotation = 15, UnitWidth = TimeSpan.FromDays(1).Ticks, ShowSeparatorLines = true, MinStep = 1, } },
            new List<ICartesianAxis> { new Axis { UnitWidth = 0, ShowSeparatorLines = true, MinStep = 1, } }
        ));
        Charts.Add(new ChartViewModel("Азот",
            new ISeries[] { new LineSeries<double> { Name = "Азот", Values = sensors.Select(x => x.Nitrogen).ToList(), Fill = null } },
            new[] { new RectangularSection { Yi = actualStatus.StartingValueNitrogen, Yj = actualStatus.EndingValueNitrogen, Fill = new SolidColorPaint { Color = SKColors.LimeGreen.WithAlpha(60) } } },
            new List<ICartesianAxis> { new Axis { LabelsRotation = 15, UnitWidth = TimeSpan.FromDays(1).Ticks, ShowSeparatorLines = true, MinStep = 1, } },
            new List<ICartesianAxis> { new Axis { UnitWidth = 0, ShowSeparatorLines = true, MinStep = 1, } }
        ));
    }
}