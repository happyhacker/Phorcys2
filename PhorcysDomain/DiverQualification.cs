using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiverQualification
{
    public int DiverId { get; set; }

    public int QualificationId { get; set; }

    public DateTime? Qualified { get; set; }

    public string Notes { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Diver Diver { get; set; } = null!;

    public virtual Qualification Qualification { get; set; } = null!;
}
