using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DiverQualification
{
    public int DiverId { get; set; }

    public int QualificationId { get; set; }

    public DateOnly? Qualified { get; set; }

    public string Notes { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Diver Diver { get; set; }

    public virtual Qualification Qualification { get; set; }
}
