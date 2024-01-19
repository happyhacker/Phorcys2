using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiveTeam
{
    public int DivePlanId { get; set; }

    public int DiverId { get; set; }

    public int? RoleId { get; set; }

    public virtual DivePlan DivePlan { get; set; } = null!;

    public virtual Diver Diver { get; set; } = null!;

    public virtual Role? Role { get; set; }
}
