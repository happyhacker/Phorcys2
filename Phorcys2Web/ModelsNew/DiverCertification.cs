using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

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
    public DateOnly? Certified { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public string CertificationNum { get; set; }

    /// <summary>
    /// N
    /// </summary>
    public string Notes { get; set; }

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

    public virtual Certification Certification { get; set; }

    public virtual Diver Diver { get; set; }

    public virtual Instructor Instructor { get; set; }
}
