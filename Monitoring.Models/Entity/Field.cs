namespace Monitoring.Models.Entity;

public class Field
{
    public Field()
    {
        Seeds = new HashSet<Seed>();
    }
    
    public int Id { get; set; }
    /// <summary>
    /// Район
    /// </summary>
    public string District { get; set; }
    /// <summary>
    /// Номер района
    /// </summary>
    public string Number { get; set; }
    /// <summary>
    /// Местоположение
    /// </summary>
    public string? Position { get; set; }
    /// <summary>
    /// Тип земли
    /// </summary>
    public virtual TypeGround? TypeGround { get; set; }
    public virtual ICollection<Seed> Seeds { get; set; }
}