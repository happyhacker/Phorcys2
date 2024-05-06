using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DiveType
{
    public int DiveTypeId { get; set; }

    public string Title { get; set; }

    public string Notes { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<DivePlan> DivePlans { get; set; } = new List<DivePlan>();
}
