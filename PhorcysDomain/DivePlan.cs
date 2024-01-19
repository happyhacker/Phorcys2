using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

/// <summary>
/// N
/// </summary>
public partial class DivePlan
{
    /// <summary>
    /// N
    /// </summary>
    public int DivePlanId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int? DiveSiteId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// N
    /// </summary>
    public DateTime ScheduledTime { get; set; }

    public int? MaxDepth { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public DateTime LastModified { get; set; }

    public virtual DiveSite? DiveSite { get; set; }

    //public virtual ICollection<DiveTeam> DiveTeams { get; set; } = new List<DiveTeam>();

    //public ICollection<Dive> Dives { get; set; } = new List<Dive>();

    //public virtual ICollection<TanksOnDive> TanksOnDives { get; set; } = new List<TanksOnDive>();

    public required User User { get; set; }

    //public virtual ICollection<DiveType> DiveTypes { get; set; } = new List<DiveType>();

    //public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();
}
