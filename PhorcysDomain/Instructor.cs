using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

/// <summary>
/// N
/// </summary>
public partial class Instructor
{
    /// <summary>
    /// N
    /// </summary>
    public int InstructorId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int ContactId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public string? Notes { get; set; }

    public virtual ICollection<AgencyInstructor> AgencyInstructors { get; set; } = new List<AgencyInstructor>();

    public virtual Contact Contact { get; set; } = null!;

    public virtual ICollection<DiverCertification> DiverCertifications { get; set; } = new List<DiverCertification>();
}
