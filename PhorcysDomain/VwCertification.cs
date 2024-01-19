using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class VwCertification
{
    public int CertificationId { get; set; }

    public string Title { get; set; } = null!;

    public int DiverId { get; set; }

    public string Agency { get; set; } = null!;

    public DateTime? Certified { get; set; }

    public string CertificationNum { get; set; } = null!;

    public string DiverFirstName { get; set; } = null!;

    public string DiverLastName { get; set; } = null!;

    public string? InstructorFirstName { get; set; }

    public string? InstructorLastName { get; set; }
}
