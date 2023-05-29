using System;

namespace SystemMonitoringNetCore.Models;

public class SensorData
{
    public DateTime DateTime { get; set; }

    public Sensor Info { get; set; }

    public SensorData(Sensor sensor)
    {
        DateTime = DateTime.Now;
        Info = sensor;
    }
}