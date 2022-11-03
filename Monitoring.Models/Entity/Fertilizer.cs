using System.ComponentModel.DataAnnotations.Schema;

namespace Monitoring.Models.Entity;

/// <summary>
/// Класс удобрений
/// </summary>
public class Fertilizer
{
    /// <summary>
    /// 
    /// </summary>
    public Fertilizer()
    {
        SeedFertilizers = new HashSet<SeedFertilizer>();
    }
    
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Количество азота
    /// </summary>
    public string? Asot { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PhosphorusOxide { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PotassiumOxide { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? CalciumOxide { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? MagnesiumOxide { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? SulfurOxide { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Borum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Sodium { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Zincum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Cuprum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Manganum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [NotMapped]
    public virtual ICollection<SeedFertilizer> SeedFertilizers { get; set; }
}