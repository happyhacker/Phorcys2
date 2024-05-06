using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Service
{
    public int ServiceId { get; set; }

    public string Title { get; set; }

    public string Notes { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<DiveShop> DiveShops { get; set; } = new List<DiveShop>();
}
