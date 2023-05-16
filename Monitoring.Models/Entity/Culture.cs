namespace Monitoring.Models.Entity;

public class Culture
{
    public Culture()
    {
        Seeds = new HashSet<Seed>();
        Statuses = new HashSet<CultureStatus>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Seed> Seeds { get; set; }
    public virtual ICollection<CultureStatus> Statuses { get; set; }
}