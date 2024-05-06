using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Country
{
    public string CountryCode { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
