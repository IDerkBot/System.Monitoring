using System;
using System.IO.Ports;

namespace SystemMonitoringNetCore.Models;

/// <summary>
/// Датчик
/// </summary>
public class Sensor
{
    #region Variables

    private SerialPort _serialPort = new SerialPort("COM8", 9600);

    public Sensor()
    {
        
    }
    public Sensor(string lineData)
    {
        var data = lineData.Split(';');
        Id = int.Parse(data[0]);
        Temperature = double.Parse(data[1].ChangeValue());
        Humidity = double.Parse(data[2]);
        Acidity = double.Parse(data[3]);
    }

    /// <summary> Идентификатор датчика </summary>
    public int Id { get; set; }

    /// <summary> Влажность </summary>
    public double Humidity { get; set; }

    /// <summary> Температура </summary>
    public double Temperature { get; set; }

    /// <summary> Кислотность </summary>
    public double Acidity { get; set; }

    /// <summary> Фосфор </summary>
    public double Phosphorus { get; set; }

    /// <summary> Калий </summary>
    public double Potassium { get; set; }

    /// <summary> Засоленность </summary>
    public double Salinity { get; set; }
    /// <summary> Натрий </summary>
    public double Sodium { get; set; }

    /// <summary> Рекомендации </summary>
    public string Recommendation { get; set; }

    #endregion


    public void GetData()
    {
    }

    public override string ToString()
    {
        return $"Sensor: {Id}\n" +
               $"Temperature: {Temperature}\n" +
               $"Acidity: {Acidity}\n";
    }
}

internal static class ChangeData
{
    private const string CURRENT_SEPARATOR = ",";
    private const string NOT_VALID_SEPARATOR = ".";

    internal static string ChangeValue(this string value)
    {
        if (value.Contains(NOT_VALID_SEPARATOR))
        {
            var index = value.IndexOf(NOT_VALID_SEPARATOR, StringComparison.Ordinal);
            return value.Substring(0, index) + CURRENT_SEPARATOR + value.Substring(index + 1);
        }
        else
            return value;
    }
}