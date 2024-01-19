using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

/// <summary>
/// N
/// </summary>
public partial class GasMix
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
    public int GasId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int? VolumeAdded { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public double? Percentage { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public decimal? CostPerVolumeOfMeasure { get; set; }

    public virtual Gase Gas { get; set; } = null!;

    public virtual TanksOnDive TanksOnDive { get; set; } = null!;
}
