namespace SystemMonitoringNetCore.Models;

public class SensorDataJson
{
    // {"uid":2483816535,"ms":0,"tm":268,"con":0,"ph":30,"nc":0,"phc":0,"poc":0,"sal":0,"tds":0}
    public uint uid { get; set; }
    public int ms { get; set; }
    public int tm { get; set; }
    public int con { get; set; }
    public int ph { get; set; }
    public int nc { get; set; }
    public int phc { get; set; }
    public int poc { get; set; }
    public int sal { get; set; }
    public int tds { get; set; }
}