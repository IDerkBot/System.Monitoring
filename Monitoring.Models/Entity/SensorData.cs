namespace Monitoring.Models.Entity;

public class SensorData
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }

    public Sensor SensorInfo { get; set; }
    public string CultureStatus { get; set; }

    public SensorData()
    {
        
    }
}