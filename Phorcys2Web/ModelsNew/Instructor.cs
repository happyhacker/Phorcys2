using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

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
    public string Notes { get; set; }

    public virtual Contact Contact { get; set; }

    public virtual ICollection<DiverCertification> DiverCertifications { get; set; } = new List<DiverCertification>();

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();
}
