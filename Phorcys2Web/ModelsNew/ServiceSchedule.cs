using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class ServiceSchedule
{
    public int ServiceScheduleId { get; set; }

    public int GearId { get; set; }

    public string Title { get; set; }

    public DateOnly? NextServiceDate { get; set; }

    public string Notes { get; set; }

    public virtual Gear Gear { get; set; }
}
