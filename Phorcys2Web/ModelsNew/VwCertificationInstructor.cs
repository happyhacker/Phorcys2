using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class VwCertificationInstructor
{
    public int ContactId { get; set; }

    public string Company { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int InstructorId { get; set; }

    public int DiveAgencyId { get; set; }

    public string Agency { get; set; }

    public string Certification { get; set; }

    public int CertificationId { get; set; }
}
