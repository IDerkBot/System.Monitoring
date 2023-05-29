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
    
    /// <summary> Количество азота </summary>
    public double Asot { get; set; }
    /// <summary> </summary>
    public double PhosphorusOxide { get; set; }
    /// <summary> </summary>
    public double PotassiumOxide { get; set; }
    /// <summary> </summary>
    public double CalciumOxide { get; set; }
    /// <summary> </summary>
    public double MagnesiumOxide { get; set; }
    /// <summary> </summary>
    public double SulfurOxide { get; set; }
    /// <summary> </summary>
    public double Borum { get; set; }
    /// <summary> </summary>
    public double Sodium { get; set; }
    /// <summary> </summary>
    public double Zincum { get; set; }
    /// <summary> </summary>
    public double Cuprum { get; set; }
    /// <summary> </summary>
    public double Manganum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [NotMapped]
    public virtual ICollection<SeedFertilizer> SeedFertilizers { get; set; }
}