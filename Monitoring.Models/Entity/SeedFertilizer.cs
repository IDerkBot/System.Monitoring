using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Monitoring.Models.Entity;

/// <summary>
/// Смежная таблица между посевами и удобрениями
/// </summary>
[Keyless]
public class SeedFertilizer
{
    // public int IdSeed { get; set; }
    // public int IdFertilizer { get; set; }
    /// <summary>
    /// Удобрения
    /// </summary>
    public virtual Fertilizer Fertilizer { get; set; }
    /// <summary>
    /// Поле с посевами
    /// </summary>
    public virtual Seed Seed { get; set; }
    /// <summary>
    /// Количество использованного удобрения
    /// </summary>
    public double? Count { get; set; }
    
}