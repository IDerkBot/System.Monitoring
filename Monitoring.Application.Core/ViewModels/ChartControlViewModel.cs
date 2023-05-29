using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using Newtonsoft.Json;
using SystemMonitoringNetCore.Models;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class ChartControlViewModel : BaseViewModel
{
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

    private void UpdateChart()
    {
        if(string.IsNullOrWhiteSpace(_selectedData)) return;
        switch (_selectedData)
            {
                case "Температура":
                    Series = new ISeries[]
                    {
                        new LineSeries<DateTimePoint>
                        {
                            Values = _allSensors.Where(x => x.Info.Id == SelectedSensor.Id)
                                .Select(x => new DateTimePoint(x.DateTime, x.Info.Temperature))
                                .ToList(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                        }
                    };
                    break;
                case "Натрий":
                    Series = new ISeries[]
                    {
                        new LineSeries<DateTimePoint>
                        {
                            Values = _allSensors.Where(x => x.Info.Id == SelectedSensor.Id)
                                .Select(x => new DateTimePoint(x.DateTime, x.Info.Sodium))
                                .ToList(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                        }
                    };
                    break;
                case "Фосфор":
                    Series = new ISeries[]
                    {
                        new LineSeries<DateTimePoint>
                        {
                            Values = _allSensors.Where(x => x.Info.Id == SelectedSensor.Id)
                                .Select(x => new DateTimePoint(x.DateTime, x.Info.Phosphorus))
                                .ToList(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                        }
                    };
                    break;
                case "Засоленность":
                    Series = new ISeries[]
                    {
                        new LineSeries<DateTimePoint>
                        {
                            Values = _allSensors.Where(x => x.Info.Id == SelectedSensor.Id)
                                .Select(x => new DateTimePoint(x.DateTime, x.Info.Salinity))
                                .ToList(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                        }
                    };
                    break;
                case "Влажность":
                    Series = new ISeries[]
                    {
                        new LineSeries<DateTimePoint>
                        {
                            Values = _allSensors.Where(x => x.Info.Id == SelectedSensor.Id)
                                .Select(x => new DateTimePoint(x.DateTime, x.Info.Humidity))
                                .ToList(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                        }
                    };
                    break;
                case "Кислотность":
                    Series = new ISeries[]
                    {
                        new LineSeries<DateTimePoint>
                        {
                            Values = _allSensors.Where(x => x.Info.Id == SelectedSensor.Id)
                                .Select(x => new DateTimePoint(x.DateTime, x.Info.Acidity))
                                .ToList(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                        }
                    };
                    break;
                case "Калий":
                    Series = new ISeries[]
                    {
                        new LineSeries<DateTimePoint>
                        {
                            Values = _allSensors.Where(x => x.Info.Id == SelectedSensor.Id)
                                .Select(x => new DateTimePoint(x.DateTime, x.Info.Potassium))
                                .ToList(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                        }
                    };
                    break;
            }
        
        XAxis = new List<ICartesianAxis>
        {
            new Axis
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long)value).ToString("dd.MM.yyyy HH:mm"),
                UnitWidth = TimeSpan.FromHours(1).Ticks
            }
        };

        YAxis = new List<ICartesianAxis>
        {
            new Axis
            {
                MinStep = 1
            }
        };
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

    private readonly List<SensorData> _allSensors;

    public ChartControlViewModel(List<Sensor> sensors)
    {
        Sensors = new ObservableCollection<Sensor>(sensors);
        var str = File.ReadAllText(Constants.SensorsData);
        var items = JsonConvert.DeserializeObject<List<SensorData>>(str);
        if(items?.Count > 0) _allSensors = items;

        DataList = new List<string> { "Температура", "Натрий", "Калий", "Фосфор", "Засоленность", "Влажность", "Кислотность" };
    }
}