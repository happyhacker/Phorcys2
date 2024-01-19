using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiveUrl
{
    public int DiveUrlId { get; set; }

    public int DiveId { get; set; }

    public string Url { get; set; } = null!;

    public bool IsImage { get; set; }

    public string? Title { get; set; }

    public virtual Dive Dive { get; set; } = null!;
}
