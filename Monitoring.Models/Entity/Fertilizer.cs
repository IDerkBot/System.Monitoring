using System.ComponentModel.DataAnnotations.Schema;

namespace Monitoring.Models.Entity;

/// <summary> Класс удобрений </summary>
public class Fertilizer
{
    /// <summary> </summary>
    public Fertilizer()
    {
        SeedFertilizers = new HashSet<SeedFertilizer>();
    }
    
    /// <summary> Идентификатор </summary>
    public int Id { get; set; }

    public string Name { get; set; }
    
    /// <summary> азот </summary>
    public double? Nitrogen { get; set; }
    /// <summary> </summary>
    public double? Phosphor { get; set; }
    /// <summary> </summary>
    public double? Potassium { get; set; }
    /// <summary> </summary>
    public double? Calcium { get; set; }
    /// <summary> </summary>
    public double? Magnesium { get; set; }

    public double? Ph { get; set; }
    public int Kg { get; set; }
    [NotMapped]
    public virtual ICollection<SeedFertilizer> SeedFertilizers { get; set; }
}