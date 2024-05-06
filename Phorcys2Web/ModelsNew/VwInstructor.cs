using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class VwInstructor
{
    public int InstructorId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string CountryCode { get; set; }

    public string Email { get; set; }

    public string Agency { get; set; }

    public string InstructorAgencyId { get; set; }
}
