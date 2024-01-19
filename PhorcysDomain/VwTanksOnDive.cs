using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class VwTanksOnDive
{
    public int GearId { get; set; }

    public int DiveDetailsId { get; set; }

    public int? Volume { get; set; }

    public int? WorkingPressure { get; set; }

    public int? StartingPressure { get; set; }

    public int? EndingPressure { get; set; }

    public int? FillVolume { get; set; }

    public int? Thirds { get; set; }

    public int? EndingVolume { get; set; }

    public int? GasUsed { get; set; }
}
