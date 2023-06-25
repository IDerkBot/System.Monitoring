using System.ComponentModel.DataAnnotations;

namespace Monitoring.Models.Entity;

/// <summary> Датчик </summary>
public class Sensor
{
    public Sensor()
    {
        SensorData = new HashSet<SensorData>();
    }

    /// <summary> Идентификатор датчика </summary>
    [Key]
    public uint Uid { get; set; }

    public double PositionX { get; set; }
    public double PositionY { get; set; }
    
    public Field Field { get; set; }

    public virtual ICollection<SensorData> SensorData { get; set; }

}