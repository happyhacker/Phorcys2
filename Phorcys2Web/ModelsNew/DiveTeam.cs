using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DiveTeam
{
    public int DivePlanId { get; set; }

    public int DiverId { get; set; }

    public int? RoleId { get; set; }

    public virtual DivePlan DivePlan { get; set; }

    public virtual Diver Diver { get; set; }

    public virtual Role Role { get; set; }
}
