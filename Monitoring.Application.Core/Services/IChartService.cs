using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using Monitoring.Models.Entity;

namespace SystemMonitoringNetCore.Services;

public interface IChartService
{
    ISeries[] Series { get; set; }
    RectangularSection[] Sections { get; set; }
    List<ICartesianAxis> AxisX { get; set; }
    List<ICartesianAxis> AxisY { get; set; }

    void ReadData(List<SensorData> sensorData, string selectedValue);
}