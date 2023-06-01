﻿using System.Drawing;
using System.Security.AccessControl;

namespace Monitoring.Models.Entity;

public class Field
{
    public Field()
    {
        Seeds = new HashSet<Seed>();
    }
    
    public int Id { get; set; }
    /// <summary> Номер района </summary>
    public string Number { get; set; }
    /// <summary> Местоположение </summary>
    public double? PositionX { get; set; }
    public double? PositionY { get; set; }

    public virtual District District { get; set; }
    public virtual ICollection<Seed> Seeds { get; set; }
}