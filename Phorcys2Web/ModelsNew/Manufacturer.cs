using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public int ContactId { get; set; }

    public virtual Contact Contact { get; set; }

    public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();
}
