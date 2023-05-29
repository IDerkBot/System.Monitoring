namespace Monitoring.Models.Entity;

public class CultureStatus
{
    public int Id { get; set; }
    public string Status { get; set; }
    public int StartPeriod { get; set; }
    public int EndPeriod { get; set; }
    public double StartingValuePh { get; set; }
    public double EndingValuePh { get; set; }
    public double StartingValuePhosphor { get; set; }
    public double EndingValuePhosphor { get; set; }
    public double StartingValuePotassium { get; set; }
    public double EndingValuePotassium { get; set; }
    public double StartingValueMagnesium { get; set; }
    public double EndingValueMagnesium { get; set; }
    public double StartingValueCalcium { get; set; }
    public double EndingValueCalcium { get; set; }
    public double StartingValueHumidity { get; set; }
    public double EndingValueHumidity { get; set; }
    public double StartingValueNitrogen { get; set; }
    public double EndingValueNitrogen { get; set; }
    public double StartingValueTemperature { get; set; }
    public double EndingValueTemperature { get; set; }
    public Culture Culture { get; set; }
}