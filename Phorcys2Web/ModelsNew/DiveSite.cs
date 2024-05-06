using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DiveSite
{
    public int DiveSiteId { get; set; }

    public int? DiveLocationId { get; set; }

    public string Title { get; set; }

    public bool IsFreshWater { get; set; }

    public string GeoCode { get; set; }

    public string Notes { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public int? MaxDepth { get; set; }

    public virtual DiveLocation DiveLocation { get; set; }

    public virtual ICollection<DivePlan> DivePlans { get; set; } = new List<DivePlan>();

    public virtual ICollection<DiveSiteUrl> DiveSiteUrls { get; set; } = new List<DiveSiteUrl>();

    public virtual User User { get; set; }
}
