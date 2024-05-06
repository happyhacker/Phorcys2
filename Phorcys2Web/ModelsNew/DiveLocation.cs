using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DiveLocation
{
    public int DiveLocationId { get; set; }

    public int? ContactId { get; set; }

    public string Title { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public string Notes { get; set; }

    public virtual Contact Contact { get; set; }

    public virtual ICollection<DiveSite> DiveSites { get; set; } = new List<DiveSite>();

    public virtual User User { get; set; }
}
