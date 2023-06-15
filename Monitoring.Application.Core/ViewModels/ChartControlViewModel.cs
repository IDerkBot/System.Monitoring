using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using Monitoring.Models.Entity;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.Services;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class ChartControlViewModel : BaseViewModel
{
    #region Properties

    #region Sensors : ObservableCollection<Sensor> - Список датчиков

    private ObservableCollection<Sensor> _sensors;

    /// <summary> Список датчиков </summary>
    public ObservableCollection<Sensor> Sensors
    {
        get => _sensors;
        set => SetField(ref _sensors, value);
    }

    #endregion Sensors

    #region SelectedSensor : Sensor - Выбранный датчик

    private Sensor _selectedSensor;

    /// <summary> Выбранный датчик </summary>
    public Sensor SelectedSensor
    {
        get => _selectedSensor;
        set
        {
            SetField(ref _selectedSensor, value);
            UpdateChart();
        }
    }

    #endregion SelectedSensor

    #region DataList : List<string> - Типы данных

    private List<string> _dataList;

    /// <summary> Типы данных </summary>
    public List<string> DataList
    {
        get => _dataList;
        set => SetField(ref _dataList, value);
    }

    #endregion DatasList

    #region CurrentCultureStatus : CultureStatus - Корректный статус культуры для выбранных сенсоров

    private CultureStatus _currentCultureStatus;

    /// <summary> Корректный статус культуры для выбранных сенсоров </summary>
    public CultureStatus CurrentCultureStatus
    {
        get => _currentCultureStatus;
        set => SetField(ref _currentCultureStatus, value);
    }

    #endregion CurrentCultureStatus

    #region Section : RectangularSection[] - Секции

    private RectangularSection[] _section;

    /// <summary> Секции </summary>
    public RectangularSection[] Section
    {
        get => _section;
        set => SetField(ref _section, value);
    }

    #endregion Section

    #region SelectedData : string - Выбранные тип данных

    private string _selectedData;

    /// <summary> Выбранные тип данных </summary>
    public string SelectedData
    {
        get => _selectedData;
        set
        {
            SetField(ref _selectedData, value);
            UpdateChart();
        }
    }

    #endregion SelectedData

    #region XAxis : List<ICartesianAxis> - Список дат

    private List<ICartesianAxis> _xAxis;

    /// <summary> Список дат </summary>
    public List<ICartesianAxis> XAxis
    {
        get => _xAxis;
        set => SetField(ref _xAxis, value);
    }

    #endregion Dates

    #region YAxis : List<ICartesianAxis> - Ось Y

    private List<ICartesianAxis> _yAxis;

    /// <summary> Ось Y </summary>
    public List<ICartesianAxis> YAxis
    {
        get => _yAxis;
        set => SetField(ref _yAxis, value);
    }

    #endregion YAxis

    #region Series : ISeries[] - Данные для графика

    private ISeries[] _series;

    /// <summary> Данные для графика </summary>
    public ISeries[] Series
    {
        get => _series;
        set => SetField(ref _series, value);
    }

    #endregion Series

    #region StartDate : DateTime - Дата от

    private DateTime _startDate;

    /// <summary> Дата от </summary>
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            SetField(ref _startDate, value);
            UpdateChart();
        }
    }

    #endregion StartDate

    #region EndDate : DateTime - Дата до

    private DateTime _endDate;

    /// <summary> Дата до </summary>
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            SetField(ref _endDate, value);
            UpdateChart();
        }
    }

    #endregion EndDate

    #endregion

    #region Private Properties

    private readonly IChartService _chartService;

    #endregion Private Properties
    
    public ChartControlViewModel(List<Sensor> sensors, CultureStatus currentCultureStatus)
    {
        _chartService = Ioc.Default.GetService<IChartService>();
        StartDate = DateTime.Now.AddDays(-183);
        EndDate = DateTime.Now.AddDays(183);
        Sensors = new ObservableCollection<Sensor>(sensors);
        CurrentCultureStatus = currentCultureStatus;

        DataList = new List<string> { "Температура", "Натрий", "Калий", "Фосфор", "Засоленность", "Влажность", "Кислотность" };
    }

    #region Private Methods

    private void UpdateChart()
    {
        if (string.IsNullOrWhiteSpace(_selectedData)) return;
        
        // Получает все данные датчика с указанным Id из таблицы SensorData (Записей от 1 до 1.000.000.000.000.000.000.000.000)
        var data = Db.DbContext.SensorData.Where(x => x.Sensor.Id == SelectedSensor.Id).ToList();
        // Выбираем из списка данных что мы получили выше ^, все данные которые входят в указанный диапазон дат
        var currentSensorData = data.Where(x => StartDate <= x.DateTime && EndDate >= x.DateTime).ToList();
        // Если данных нет, то выходим из метода
        if (!currentSensorData.Any()) return;

        // Обращаемся к сервису который считывает заданную информацию и строит показатели для графика
        _chartService.ReadData(currentSensorData, SelectedData);

        // Берем показатели из сервиса
        Series = _chartService.Series;
        Section = _chartService.Sections;
        YAxis = _chartService.AxisY;
        XAxis = _chartService.AxisX;

        OnPropertyChanged(nameof(Series));
        OnPropertyChanged(nameof(Section));
        OnPropertyChanged(nameof(YAxis));
        OnPropertyChanged(nameof(XAxis));
    }

    #endregion
}