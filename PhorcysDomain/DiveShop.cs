using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class DiveShop
{
    public int DiveShopId { get; set; }


    public string? Notes { get; set; }

    //public virtual Contact Contact { get; set; } = null!;
    //public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
	public Contact? Contact { get; set; } // Navigation property back to Contact
	
    public int ContactId { get; set; }

	public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
