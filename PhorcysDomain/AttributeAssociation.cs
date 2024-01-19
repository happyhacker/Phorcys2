using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class AttributeAssociation
{
    public int AttributeId { get; set; }

    public int TableRowId { get; set; }

    public string Title { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;
}
