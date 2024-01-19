using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class ServiceSchedule
{
    public int ServiceScheduleId { get; set; }

    public int GearId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime? NextServiceDate { get; set; }

    public string? Notes { get; set; }

    public virtual Gear Gear { get; set; } = null!;
}
