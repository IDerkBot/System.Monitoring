namespace Monitoring.Models.Entity;

public class District
{
    public District()
    {
        Fields = new HashSet<Field>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public double LocationX { get; set; }
    public double LocationY { get; set; }
    public virtual ICollection<Field> Fields { get; set; }
}