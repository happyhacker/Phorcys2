using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

/// <summary>
/// N
/// </summary>
public partial class DiverCertification
{
    /// <summary>
    /// N
    /// </summary>
    public int DiverCertificationId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int DiverId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int CertificationId { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public DateTime? Certified { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public string CertificationNum { get; set; } = null!;

    /// <summary>
    /// N
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public int? InstructorId { get; set; }

    public virtual Certification Certification { get; set; } = null!;

    public virtual Diver Diver { get; set; } = null!;

    public virtual Instructor? Instructor { get; set; }
}
