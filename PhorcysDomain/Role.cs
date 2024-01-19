using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class Role
{
    public int RoleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Notes { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<DiveTeam> DiveTeams { get; set; } = new List<DiveTeam>();

    public virtual User User { get; set; } = null!;
}
