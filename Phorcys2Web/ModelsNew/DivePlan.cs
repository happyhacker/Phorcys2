using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DivePlan
{
    public int DivePlanId { get; set; }

    public int? DiveSiteId { get; set; }

    public string Title { get; set; }

    public int? Minutes { get; set; }

    public DateTime ScheduledTime { get; set; }

    public int? MaxDepth { get; set; }

    public string Notes { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual DiveSite DiveSite { get; set; }

    public virtual ICollection<DiveTeam> DiveTeams { get; set; } = new List<DiveTeam>();

    public virtual ICollection<Dife> Dives { get; set; } = new List<Dife>();

    public virtual ICollection<TanksOnDive> TanksOnDives { get; set; } = new List<TanksOnDive>();

    public virtual User User { get; set; }

    public virtual ICollection<DiveType> DiveTypes { get; set; } = new List<DiveType>();

    public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();
}
