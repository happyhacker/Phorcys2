using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class TanksOnDive
{

    public int DivePlanId { get; set; }

    public int GearId { get; set; }

    public string? GasContentTitle { get; set; }

    public int StartingPressure { get; set; } = 0;

    public int EndingPressure { get; set; } = 0;

    public int OxygenPercent { get; set; } = 0;

    public int HeliumPercent { get; set; } = 0;

    public decimal? FillCost { get; set; }

    public DateTime? FillDate { get; set; }

    public virtual DivePlan DivePlan { get; set; } = null!;

    public virtual Tank Tank { get; set; } = null!;

    //public virtual ICollection<GasMix> GasMixes { get; set; } = new List<GasMix>();

}
