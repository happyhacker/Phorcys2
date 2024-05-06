using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Dife
{
    public int DiveId { get; set; }

    public int? DivePlanId { get; set; }

    public string Title { get; set; }

    public int? Minutes { get; set; }

    public DateTime? DescentTime { get; set; }

    public int? AvgDepth { get; set; }

    public int? MaxDepth { get; set; }

    public int? Temperature { get; set; }

    public int? AdditionalWeight { get; set; }

    public string Notes { get; set; }

    public int DiveNumber { get; set; }

    public int? UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual DivePlan DivePlan { get; set; }

    public virtual ICollection<DiveUrl> DiveUrls { get; set; } = new List<DiveUrl>();

    public virtual User User { get; set; }
}
