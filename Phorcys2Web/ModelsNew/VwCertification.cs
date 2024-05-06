using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class VwCertification
{
    public int CertificationId { get; set; }

    public string Title { get; set; }

    public int DiverId { get; set; }

    public string Agency { get; set; }

    public DateOnly? Certified { get; set; }

    public string CertificationNum { get; set; }

    public string DiverFirstName { get; set; }

    public string DiverLastName { get; set; }

    public string InstructorFirstName { get; set; }

    public string InstructorLastName { get; set; }
}
