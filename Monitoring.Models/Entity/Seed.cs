using System.ComponentModel.DataAnnotations.Schema;

namespace Monitoring.Models.Entity;

public class Seed
{
    public Seed()
    {
        SeedFertilizers = new HashSet<SeedFertilizer>();
    }
    
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public Culture? Culture { get; set; }
    public Field Field { get; set; }
    public TypeGround? TypeGround { get; set; }
    public CultureStatus? Status { get; set; }
    [NotMapped]
    public virtual ICollection<SeedFertilizer> SeedFertilizers { get; set; }
}