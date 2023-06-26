using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Monitoring.Models.Entity;
using SkiaSharp;
using SystemMonitoringNetCore.Models;

namespace SystemMonitoringNetCore.Services;

public class ChartService : IChartService
{
    public ISeries[] Series { get; set; }
    public RectangularSection[] Sections { get; set; }
    public List<ICartesianAxis> AxisX { get; set; }
    public List<ICartesianAxis> AxisY { get; set; }

    public ChartService()
    {
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

        AxisX = new List<ICartesianAxis>
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

        AxisY = new List<ICartesianAxis>
        {
            new Axis
            {
                MinStep = 1,
                ShowSeparatorLines = true,
            }
        };
    }

    public void ReadData(List<SensorData> sensorData, string selectedValue) => GetData(selectedValue, sensorData.OrderBy(x => x.DateTime).ToList());

    private void GetData(string selectedData, List<SensorData> sensorData)
    {
        var statuses = sensorData.GroupBy(x => x.CultureStatus).ToList();
        var countStatuses = statuses.Count;

        Sections = new RectangularSection[countStatuses];
        long lastMaxDate = 0;

        for (var i = 0; i < countStatuses; i++)
        {
            var currentData = sensorData.Where(x => x.CultureStatus == statuses[i].Key).ToList();
            var minDate = lastMaxDate == 0 ? currentData.Min(x => x.DateTime).Ticks : lastMaxDate;
            var maxDate = currentData.Max(x => x.DateTime).Ticks;
            lastMaxDate = maxDate;
            Sections[i] = new RectangularSection
            {
                Xi = minDate,
                Xj = maxDate,
                Fill = new SolidColorPaint { Color = i % 2 == 0 ? SKColors.LimeGreen.WithAlpha(60) : SKColors.Green.WithAlpha(60) }
            };
        }

        switch (selectedData)
        {
            case "Температура":
                Series[0].Name = "Температура";
                Series[0].Values = sensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.Temperature))
                    .ToList();

                AxisY[0].MaxLimit = sensorData.Max(x => x.Temperature) + 10;
                AxisY[0].MinLimit = sensorData.Min(x => x.Temperature) - 10;

                for (var i = 0; i < countStatuses; i++)
                {
                    if (statuses[i].Key == "не выбрано") continue;
                    if (!Db.DbContext.CultureStatuses.Any(x => x.Status == statuses[i].Key)) continue;
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Sections[i].Yi = currentStatus.StartingValueTemperature;
                    Sections[i].Yj = currentStatus.EndingValueTemperature;
                }

                break;
            case "Азот":
                Series[0].Name = "Азот";
                Series[0].Values = sensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.Sodium))
                    .ToList();

                AxisY[0].MaxLimit = sensorData.Max(x => x.Sodium) + 10;
                AxisY[0].MinLimit = sensorData.Min(x => x.Sodium) - 10;

                for (var i = 0; i < countStatuses; i++)
                {
                    if (statuses[i].Key == "не выбрано") continue;
                    if (!Db.DbContext.CultureStatuses.Any(x => x.Status == statuses[i].Key)) continue;
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Sections[i].Yi = currentStatus.StartingValueNitrogen;
                    Sections[i].Yj = currentStatus.EndingValueNitrogen;
                }

                break;
            case "Фосфор":
                Series[0].Name = "Фосфор";
                Series[0].Values = sensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.Phosphorus))
                    .ToList();

                AxisY[0].MaxLimit = sensorData.Max(x => x.Phosphorus) + 10;
                AxisY[0].MinLimit = sensorData.Min(x => x.Phosphorus) - 10;

                for (var i = 0; i < countStatuses; i++)
                {
                    if (statuses[i].Key == "не выбрано") continue;
                    if (!Db.DbContext.CultureStatuses.Any(x => x.Status == statuses[i].Key)) continue;
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Sections[i].Yi = currentStatus.StartingValuePhosphor;
                    Sections[i].Yj = currentStatus.EndingValuePhosphor;
                }

                break;
            case "Засоленность":
                Series[0].Name = "Засоленность";
                Series[0].Values = sensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.Salinity))
                    .ToList();

                AxisY[0].MaxLimit = sensorData.Max(x => x.Salinity) + 10;
                AxisY[0].MinLimit = sensorData.Min(x => x.Salinity) - 10;

                for (var i = 0; i < countStatuses; i++)
                {
                    if (statuses[i].Key == "не выбрано") continue;
                    if (!Db.DbContext.CultureStatuses.Any(x => x.Status == statuses[i].Key)) continue;
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Sections[i].Yi = currentStatus.StartingValueSalinity;
                    Sections[i].Yj = currentStatus.EndingValueSalinity;
                }

                break;
            case "Влажность":
                Series[0].Name = "Влажность";
                Series[0].Values = sensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.Humidity))
                    .ToList();

                AxisY[0].MaxLimit = sensorData.Max(x => x.Humidity) + 10;
                AxisY[0].MinLimit = sensorData.Min(x => x.Humidity) - 10;

                for (var i = 0; i < countStatuses; i++)
                {
                    if (statuses[i].Key == "не выбрано") continue;
                    if (!Db.DbContext.CultureStatuses.Any(x => x.Status == statuses[i].Key)) continue;
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Sections[i].Yi = currentStatus.StartingValueHumidity;
                    Sections[i].Yj = currentStatus.EndingValueHumidity;
                }

                break;
            case "Кислотность":
                Series[0].Name = "Кислотность";
                Series[0].Values = sensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.Acidity))
                    .ToList();

                AxisY[0].MaxLimit = sensorData.Max(x => x.Acidity) + 10;
                AxisY[0].MinLimit = sensorData.Min(x => x.Acidity) - 10;

                for (var i = 0; i < countStatuses; i++)
                {
                    if (statuses[i].Key == "не выбрано") continue;
                    if (!Db.DbContext.CultureStatuses.Any(x => x.Status == statuses[i].Key)) continue;
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Sections[i].Yi = currentStatus.StartingValuePh;
                    Sections[i].Yj = currentStatus.EndingValuePh;
                }

                break;
            case "Калий":
                Series[0].Name = "Калий";
                Series[0].Values = sensorData
                    .Select(x => new DateTimePoint(x.DateTime, x.Potassium))
                    .ToList();

                AxisY[0].MaxLimit = sensorData.Max(x => x.Potassium) + 10;
                AxisY[0].MinLimit = sensorData.Min(x => x.Potassium) - 10;

                for (var i = 0; i < countStatuses; i++)
                {
                    if (statuses[i].Key == "не выбрано") continue;
                    if (!Db.DbContext.CultureStatuses.Any(x => x.Status == statuses[i].Key)) continue;
                    var currentStatus = Db.DbContext.CultureStatuses.First(x => x.Status == statuses[i].Key);
                    Sections[i].Yi = currentStatus.StartingValuePotassium;
                    Sections[i].Yj = currentStatus.EndingValuePotassium;
                }

                break;
        }
    }
}