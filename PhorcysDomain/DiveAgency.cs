using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiveAgency
{
    public int DiveAgencyId { get; set; }

    public int ContactId { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<AgencyInstructor> AgencyInstructors { get; set; } = new List<AgencyInstructor>();

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

    public virtual Contact Contact { get; set; } = null!;
}
