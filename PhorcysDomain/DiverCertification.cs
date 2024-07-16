using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiverCertification
{
    public int DiverCertificationId { get; set; }

    public int DiverId { get; set; }

    public int CertificationId { get; set; }

    public DateTime? Certified { get; set; }

    public string CertificationNum { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public int? InstructorId { get; set; }

    //public virtual Certification Certification { get; set; } = null!;

    //public virtual Diver Diver { get; set; } = null!;

    //public virtual Instructor? Instructor { get; set; }
}
