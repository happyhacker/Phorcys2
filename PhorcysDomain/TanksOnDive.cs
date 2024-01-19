using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

/// <summary>
/// N
/// </summary>
public partial class TanksOnDive
{
    /// <summary>
    /// N
    /// </summary>
    public int DivePlanId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int GearId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public string? GasContentTitle { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int? StartingPressure { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int? EndingPressure { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public decimal? FillCost { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public DateTime? FillDate { get; set; }

    public virtual DivePlan DivePlan { get; set; } = null!;

    public virtual ICollection<GasMix> GasMixes { get; set; } = new List<GasMix>();

    public virtual Tank Gear { get; set; } = null!;
}
