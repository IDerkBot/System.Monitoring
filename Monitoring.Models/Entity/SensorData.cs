namespace Monitoring.Models.Entity;

public class SensorData
{
    public SensorData()
    {
        
    }
    
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string CultureStatus { get; set; }
    public Sensor Sensor { get; set; }
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

    public double Nitrogen { get; set; }
    /// <summary> Рекомендации </summary>
    public string Recommendation { get; set; }
}