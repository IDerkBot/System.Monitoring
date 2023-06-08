namespace Monitoring.Models.Entity;

/// <summary>
/// Датчик
/// </summary>
public class Sensor
{
    public Sensor()
    {
        SensorData = new HashSet<SensorData>();
    }

    /// <summary> Идентификатор датчика </summary>
    public int Id { get; set; }

    public double PositionX { get; set; }
    public double PositionY { get; set; }
    
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
    
    public virtual ICollection<SensorData> SensorData { get; set; }

}