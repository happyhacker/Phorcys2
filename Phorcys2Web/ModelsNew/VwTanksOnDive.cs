using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class VwTanksOnDive
{
    public int DivePlanId { get; set; }

    public string DiveTitle { get; set; }

    public string Tank { get; set; }

    public int GearId { get; set; }

    public int? Volume { get; set; }

    public int? WorkingPressure { get; set; }

    public int? StartingPressure { get; set; }

    public int? EndingPressure { get; set; }

    public int? FillVolume { get; set; }

    public int? Thirds { get; set; }

    public int? TurnVolume { get; set; }

    public int? GasUsed { get; set; }
}
