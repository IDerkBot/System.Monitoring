using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitoring.Models.Entity;

public class TypeGround
{
    public TypeGround()
    {
        Fields = new HashSet<Field>();
    }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    
    public double StartSodium { get; set; }
    public double EndSodium { get; set; }
    public double StartPotassium { get; set; }
    public double EndPotassium { get; set; }
    public double StartPhosphorus { get; set; }
    public double EndPhosphorus { get; set; }
    public double StartHumidity { get; set; }
    public double EndHumidity { get; set; } 
    public double StartTemperature { get; set; } 
    public double EndTemperature { get; set; } 
    public double StartAcidity { get; set; } 
    public double EndAcidity { get; set; } 
    public double StartSalinity { get; set; }
    public double EndSalinity { get; set; }
    public virtual ICollection<Field> Fields { get; set; }
}