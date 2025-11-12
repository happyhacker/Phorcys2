using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class Diver
{
    public int DiverId { get; set; }

    public int ContactId { get; set; } // Foreign Key

	public virtual Contact Contact { get; set; } = null!; // Required navigation property

	public double? WorkingSacRate { get; set; }

    public string? Notes { get; set; }

    public float? RestingSacRate { get; set; }

    //public virtual Contact Contact { get; set; } = null!;

    public virtual ICollection<DiveTeam> DiveTeams { get; set; } = new List<DiveTeam>();

    //public virtual ICollection<DiverCertification> DiverCertifications { get; set; } = new List<DiverCertification>();

    //public virtual ICollection<DiverQualification> DiverQualifications { get; set; } = new List<DiverQualification>();

    //public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();
}
