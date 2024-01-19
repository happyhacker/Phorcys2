using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiveType
{
    public int DiveTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Notes { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<DivePlan> DivePlans { get; set; } = new List<DivePlan>();
}
