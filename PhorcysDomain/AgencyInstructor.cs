using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class AgencyInstructor
{
    public int InstructorId { get; set; }

    public int DiveAgencyId { get; set; }

    public string? InstructorAgencyId { get; set; }

    public virtual DiveAgency DiveAgency { get; set; } = null!;

    public virtual Instructor Instructor { get; set; } = null!;
}
