using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class TanksOnDive
{

    public int DivePlanId { get; set; }

    public int GearId { get; set; }

    public string? GasContentTitle { get; set; }

    public int? StartingPressure { get; set; }

    public int? EndingPressure { get; set; }

    public int? OxygenPercent { get; set; }

    public int? HeliumPercent { get; set; }

    public decimal? FillCost { get; set; }

    public DateTime? FillDate { get; set; }

    public virtual DivePlan DivePlan { get; set; } = null!;

    public virtual Tank Tank { get; set; } = null!;

    //public virtual ICollection<GasMix> GasMixes { get; set; } = new List<GasMix>();

}
