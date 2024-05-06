using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Gase
{
    public int GasId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<GasMix> GasMixes { get; set; } = new List<GasMix>();
}
