using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class GearServiceEvent
{
    public int GearServiceEventsId { get; set; }

    public int GearId { get; set; }

    public string Title { get; set; }

    public DateOnly ServicedDate { get; set; }

    public decimal? Cost { get; set; }

    public string Notes { get; set; }

    public virtual Gear Gear { get; set; }
}
