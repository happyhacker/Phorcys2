using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DiveAgency
{
    public int DiveAgencyId { get; set; }

    public int ContactId { get; set; }

    public string Notes { get; set; }

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

    public virtual Contact Contact { get; set; }
}
