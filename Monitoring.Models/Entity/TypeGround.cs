using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitoring.Models.Entity;

public class TypeGround
{
    public TypeGround()
    {
        Seeds = new HashSet<Seed>();
    }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public virtual ICollection<Seed> Seeds { get; set; }

    public override string ToString()
    {
        return Title;
    }
}