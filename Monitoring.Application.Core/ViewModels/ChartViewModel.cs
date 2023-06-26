using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class ChartViewModel : BaseViewModel
{
    #region ActualSeries : ISeries[] - Актуальный график

    private ISeries[] _actualSeries;

    /// <summary> Актуальный график </summary>
    public ISeries[] ActualSeries
    {
        get => _actualSeries;
        set => SetField(ref _actualSeries, value);
    }

    #endregion ActualSeries

    #region ActualSection : RectangularSection[] - Актуальная секция

    private RectangularSection[] _actualSection;

    /// <summary> Актуальная секция </summary>
    public RectangularSection[] ActualSection
    {
        get => _actualSection;
        set => SetField(ref _actualSection, value);
    }

    #endregion ActualSection

    #region ActualXAxes : List<ICartesianAxis> - Актуальная ось X

    private List<ICartesianAxis> _actualXAxes;

    /// <summary> Актуальная ось X </summary>
    public List<ICartesianAxis> ActualXAxes
    {
        get => _actualXAxes;
        set => SetField(ref _actualXAxes, value);
    }

    #endregion ActualXAxes

    #region ActualYAxes : List<ICartesianAxis> - Актуальная ось Y

    private List<ICartesianAxis> _actualYAxes;

    /// <summary> Актуальная ось Y </summary>
    public List<ICartesianAxis> ActualYAxes
    {
        get => _actualYAxes;
        set => SetField(ref _actualYAxes, value);
    }

    #endregion ActualYAxes

    #region Name : string - Название

    private string _name;

    /// <summary> Название </summary>
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    #endregion Name
    
    public ChartViewModel(string name, ISeries[] series, RectangularSection[] sections, List<ICartesianAxis> axesX, List<ICartesianAxis> axesY)
    {
        Name = name;
        ActualSeries = series;
        ActualSection = sections;
        ActualXAxes = axesX;
        ActualYAxes = axesY;
    }
}