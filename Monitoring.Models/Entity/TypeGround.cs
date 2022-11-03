namespace Monitoring.Models.Entity;

public class TypeGround
{
    public TypeGround()
    {
        Fields = new HashSet<Field>();
    }
    
    public int Id { get; set; }
    public string Title { get; set; }
    public virtual ICollection<Field> Fields { get; set; }
}