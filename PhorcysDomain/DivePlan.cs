using System;
using System.Collections.Generic;

namespace Phorcys.Domain;


public partial class DivePlan
{
    public int DivePlanId { get; set; }
    public int? DiveSiteId { get; set; }
    public string Title { get; set; } = null!;
	public int? Minutes { get; set; }
	public DateTime ScheduledTime { get; set; }
    public int? MaxDepth { get; set; }
    public string? Notes { get; set; }
    public int UserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
    public virtual DiveSite? DiveSite { get; set; }
    public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();

    //public virtual ICollection<DiveTeam> DiveTeams { get; set; } = new List<DiveTeam>();

    //public ICollection<Dive> Dives { get; set; } = new List<Dive>();

    //public virtual ICollection<TanksOnDive> TanksOnDives { get; set; } = new List<TanksOnDive>();

    public User User { get; set; }

    //public virtual ICollection<DiveType> DiveTypes { get; set; } = new List<DiveType>();


}
