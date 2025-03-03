using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class Contact
{
    public int ContactId { get; set; }

    public string Company { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string Address2 { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string? CountryCode { get; set; }

    public string Email { get; set; } = null!;

    public string CellPhone { get; set; } = null!;

    public string HomePhone { get; set; } = null!;

    public string WorkPhone { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public string Gender { get; set; } = null!;

    public string? Notes { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    //public virtual Country? CountryCodeNavigation { get; set; }

    //public virtual ICollection<DiveAgency> DiveAgencies { get; set; } = new List<DiveAgency>();

    //public virtual ICollection<DiveLocation> DiveLocations { get; set; } = new List<DiveLocation>();

    //public virtual ICollection<DiveShop> DiveShops { get; set; } = new List<DiveShop>();

    public virtual ICollection<Diver> Divers { get; set; } = new List<Diver>();

    //public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    //public virtual ICollection<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();

    //public virtual ICollection<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();

    //public virtual ICollection<SoldGear> SoldGears { get; set; } = new List<SoldGear>();

    //public virtual User User { get; set; } = null!;

    //public virtual ICollection<User> Users { get; set; } = new List<User>();

    //public virtual ICollection<DiveShop> DiveShops { get; set; } = new List<DiveShop>();
	public ICollection<DiveShop>? DiveShops { get; set; }
}
