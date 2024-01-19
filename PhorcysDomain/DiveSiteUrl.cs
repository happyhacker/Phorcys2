using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiveSiteUrl
{
    public int DiveSiteUrlId { get; set; }

    public int DiveSiteId { get; set; }

    public string Url { get; set; } = null!;

    public bool IsImage { get; set; }

    public string? Title { get; set; }

    public virtual DiveSite DiveSite { get; set; } = null!;
}
