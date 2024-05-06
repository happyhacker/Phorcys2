using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class AttributeAssociation
{
    public int AttributeId { get; set; }

    public int TableRowId { get; set; }

    public string Title { get; set; }

    public string Notes { get; set; }

    public virtual Attribute Attribute { get; set; }
}
