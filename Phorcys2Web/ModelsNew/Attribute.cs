using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public string Title { get; set; }

    public string TableToAssociate { get; set; }

    public string Notes { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<AttributeAssociation> AttributeAssociations { get; set; } = new List<AttributeAssociation>();

    public virtual User User { get; set; }
}
