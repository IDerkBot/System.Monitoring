namespace Monitoring.Models.Entity;

public class Culture
{
    // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Culture()
    {
        Seeds = new HashSet<Seed>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Period { get; set; }
    public string Ph { get; set; }
    public string Phosphor { get; set; }
    public string Potassium { get; set; }
    public string Magnesium { get; set; }
    public string Calcium { get; set; }
    public string Humidity { get; set; }
    public string Nitrogen { get; set; }
    public string Temperature { get; set; }
    
    // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Seed> Seeds { get; set; }
}