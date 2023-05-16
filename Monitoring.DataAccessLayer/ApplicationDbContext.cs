using Microsoft.EntityFrameworkCore;
using Monitoring.Models.Entity;

namespace Monitoring.DataAccessLayer;

/// <summary>
/// 
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-LSPMPFI\\SQLEXPRESS;Database=Monitoring;Trusted_Connection=True");
    }
    
    public DbSet<District> Districts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TypeGround> TypeGrounds { get; set; }
    public DbSet<Culture> Cultures { get; set; }
    public DbSet<CultureStatus> CultureStatuses { get; set; }
    public DbSet<Fertilizer> Fertilizers { get; set; }
    public DbSet<Seed> Seeds { get; set; }
    public DbSet<SeedFertilizer> SeedFertilizers { get; set; }
    public DbSet<Field> Fields { get; set; }
}