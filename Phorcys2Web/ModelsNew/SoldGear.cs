using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class SoldGear
{
    public int GearId { get; set; }

    public int? SoldToContactId { get; set; }

    public DateOnly SoldOn { get; set; }

    public decimal? Amount { get; set; }

    public string Notes { get; set; }

    public virtual Gear Gear { get; set; }

    public virtual Contact SoldToContact { get; set; }
}
