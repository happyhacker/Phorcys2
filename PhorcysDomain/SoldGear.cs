using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class SoldGear
{
    public int GearId { get; set; }

    public int? SoldToContactId { get; set; }

    public DateTime SoldOn { get; set; }

    public decimal? Amount { get; set; }

    public string? Notes { get; set; }

    public virtual Gear Gear { get; set; } = null!;

    public virtual Contact? SoldToContact { get; set; }
}
