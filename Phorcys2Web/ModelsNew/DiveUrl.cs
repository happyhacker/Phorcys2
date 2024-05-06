using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class DiveUrl
{
    public int DiveUrlId { get; set; }

    public int DiveId { get; set; }

    public string Url { get; set; }

    public bool IsImage { get; set; }

    public string Title { get; set; }

    public virtual Dife Dive { get; set; }
}
