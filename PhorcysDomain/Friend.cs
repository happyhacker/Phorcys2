using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class Friend
{
    public int RequestorUserId { get; set; }

    public int RecipientUserId { get; set; }

    public DateTime DateRequested { get; set; }

    public string? Status { get; set; }

    public DateTime LastUpdated { get; set; }

    public virtual User RecipientUser { get; set; } = null!;

    public virtual User RequestorUser { get; set; } = null!;
}
