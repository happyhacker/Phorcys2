using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class Certification
{
    public int CertificationId { get; set; }

    public int? DiveAgencyId { get; set; }

    public string Title { get; set; } = null!;

    public string? Notes { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual DiveAgency? DiveAgency { get; set; }

    public virtual ICollection<DiverCertification> DiverCertifications { get; set; } = new List<DiverCertification>();

    public virtual User User { get; set; } = null!;
}
