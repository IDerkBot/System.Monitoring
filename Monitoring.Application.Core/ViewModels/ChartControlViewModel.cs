using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Monitoring.Models.Entity;
using Newtonsoft.Json;
using SkiaSharp;
using SystemMonitoringNetCore.Models;
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

    private readonly List<SensorData> _allSensors;

    public ChartControlViewModel(List<Sensor> sensors, CultureStatus currentCultureStatus)
    {
        StartDate = DateTime.Now.AddDays(-183);
        EndDate = DateTime.Now.AddDays(183);
        Sensors = new ObservableCollection<Sensor>(sensors);
        CurrentCultureStatus = currentCultureStatus;
        var str = File.ReadAllText(Constants.SensorsData);
        // var items = JsonConvert.DeserializeObject<List<SensorData>>(str);
        var items = Db.DbContext.SensorData.ToList();
        if (items.Count > 0) _allSensors = items;

        DataList = new List<string> { "Температура", "Натрий", "Калий", "Фосфор", "Засоленность", "Влажность", "Кислотность" };
    }

    #region Private Methods

    private void UpdateChart()
    {
        if (string.IsNullOrWhiteSpace(_selectedData)) return;

        List<SensorData> currentSensorData = new List<SensorData>();
        _allSensors.ForEach(x =>
        {
            if (x.SensorInfo != null && x.SensorInfo.Id == SelectedSensor.Id)
            {
                if (StartDate <= x.DateTime && EndDate >= x.DateTime)
                {
                    currentSensorData.Add(x);
                }
            }
        });
        if (!currentSensorData.Any()) return;

        Series = new ISeries[]
        {
            new LineSeries<DateTimePoint>
            {
                Fill = null,
                IsVisibleAtLegend = true,
                LineSmoothness = .3,
                Stroke = new SolidColorPaint { Color = SKColors.Green, StrokeThickness = 2 },
                GeometryStroke = new SolidColorPaint { Color = SKColors.Green },
            }
        };
        var statuses = currentSensorData.GroupBy(x => x.CultureStatus).ToList();
        var statusCount = statuses.Count;
        Section = new RectangularSection[statusCount];
        long lastMaxDate = 0;
        
        for (var i = 0; i < statusCount; i++)
        {
            var currentData = currentSensorData.Where(x => x.CultureStatus == statuses[i].Key).ToList();
            var minDate = lastMaxDate == 0 ? currentData.Min(x => x.DateTime).Ticks : lastMaxDate;
            var maxDate = currentData.Max(x => x.DateTime).Ticks;
            lastMaxDate = maxDate;
            Section[i] = new RectangularSection
            {
                Xi = minDate,
                Xj = maxDate,
                Fill =  new SolidColorPaint { Color = i % 2 == 0 ? SKColors.LimeGreen.WithAlpha(60) : SKColors.Green.WithAlpha(60) }
            };
        }
        
        XAxis = new List<ICartesianAxis>
        {
            new Axis
            {
                LabelsRotation = 15,
                Labeler = value => value > 0 ? new DateTime((long)value).ToString("dd.MM.yyyy") : new DateTime(0).ToString("dd.MM.yyyy"),
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                ShowSeparatorLines = true,
                MinStep = 1,
            }
        };

        YAxis = new List<ICartesianAxis>
        {
            new Axis
            {
                MinStep = 1,
                ShowSeparatorLines = true,
            }
        };
        
        switch (_selectedData)
        {
            case "Температура":
                Series[0].Name = "Температура";
                Series[0].Values = currentSensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.SensorInfo.Temperature))
                    .ToList();

                YAxis[0].MaxLimit = currentSensorData.Max(x => x.SensorInfo.Temperature) + 10;
                YAxis[0].MinLimit = currentSensorData.Min(x => x.SensorInfo.Temperature) - 10;
                
                for (var i = 0; i < statusCount; i++)
                {
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Section[i].Yi = currentStatus.StartingValueTemperature;
                    Section[i].Yj = currentStatus.EndingValueTemperature;
                }
                OnPropertyChanged(nameof(Section));
                break;
            case "Натрий":
                Series[0].Name = "Натрий";
                Series[0].Values = currentSensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.SensorInfo.Sodium))
                    .ToList();
                
                YAxis[0].MaxLimit = currentSensorData.Max(x => x.SensorInfo.Sodium) + 10;
                YAxis[0].MinLimit = currentSensorData.Min(x => x.SensorInfo.Sodium) - 10;
                
                for (var i = 0; i < statusCount; i++)
                {
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Section[i].Yi = currentStatus.EndingValueTemperature;
                    Section[i].Yj = currentStatus.EndingValueTemperature;
                }
                break;
            case "Фосфор":
                Series[0].Values = currentSensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.SensorInfo.Phosphorus))
                    .ToList();
                
                YAxis[0].MaxLimit = currentSensorData.Max(x => x.SensorInfo.Phosphorus) + 10;
                YAxis[0].MinLimit = currentSensorData.Min(x => x.SensorInfo.Phosphorus) - 10;
                
                for (var i = 0; i < statusCount; i++)
                {
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Section[i].Yi = currentStatus.StartingValuePhosphor;
                    Section[i].Yj = currentStatus.EndingValuePhosphor;
                }
                break;
            case "Засоленность":
                Series[0].Values = currentSensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.SensorInfo.Salinity))
                    .ToList();
                
                YAxis[0].MaxLimit = currentSensorData.Max(x => x.SensorInfo.Salinity) + 10;
                YAxis[0].MinLimit = currentSensorData.Min(x => x.SensorInfo.Salinity) - 10;
                
                for (var i = 0; i < statusCount; i++)
                {
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Section[i].Yi = currentStatus.StartingValueTemperature;
                    Section[i].Yj = currentStatus.EndingValueTemperature;
                }
                break;
            case "Влажность":
                Series[0].Values = currentSensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.SensorInfo.Humidity))
                    .ToList();
                
                YAxis[0].MaxLimit = currentSensorData.Max(x => x.SensorInfo.Humidity) + 10;
                YAxis[0].MinLimit = currentSensorData.Min(x => x.SensorInfo.Humidity) - 10;
                
                for (var i = 0; i < statusCount; i++)
                {
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Section[i].Yi = currentStatus.StartingValueHumidity;
                    Section[i].Yj = currentStatus.EndingValueHumidity;
                }
                break;
            case "Кислотность":
                Series[0].Values = currentSensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.SensorInfo.Acidity))
                    .ToList();
                
                YAxis[0].MaxLimit = currentSensorData.Max(x => x.SensorInfo.Acidity) + 10;
                YAxis[0].MinLimit = currentSensorData.Min(x => x.SensorInfo.Acidity) - 10;
                
                for (var i = 0; i < statusCount; i++)
                {
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Section[i].Yi = currentStatus.StartingValuePh;
                    Section[i].Yj = currentStatus.EndingValuePh;
                }
                break;
            case "Калий":
                Series[0].Values = currentSensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.SensorInfo.Potassium))
                    .ToList();
                
                YAxis[0].MaxLimit = currentSensorData.Max(x => x.SensorInfo.Potassium) + 10;
                YAxis[0].MinLimit = currentSensorData.Min(x => x.SensorInfo.Potassium) - 10;
                
                for (var i = 0; i < statusCount; i++)
                {
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Section[i].Yi = currentStatus.StartingValuePotassium;
                    Section[i].Yj = currentStatus.EndingValuePotassium;
                }
                break;
        }

        OnPropertyChanged(nameof(Series));
        OnPropertyChanged(nameof(Section));
    }

    #endregion
}