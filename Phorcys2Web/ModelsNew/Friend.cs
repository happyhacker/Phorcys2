using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Friend
{
    public int RequestorUserId { get; set; }

    public int RecipientUserId { get; set; }

    public DateOnly DateRequested { get; set; }

    public string Status { get; set; }

    public DateOnly LastUpdated { get; set; }

    public virtual User RecipientUser { get; set; }

    public virtual User RequestorUser { get; set; }
}
