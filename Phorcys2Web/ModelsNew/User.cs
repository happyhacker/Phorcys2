using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class User
{
    public int UserId { get; set; }

    public string LoginId { get; set; }

    public string Password { get; set; }

    public int? LoginCount { get; set; }

    public int? ContactId { get; set; }

    public string AspNetUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastModified { get; set; }

    public virtual AspNetUser AspNetUser { get; set; }

    public virtual ICollection<Attribute> Attributes { get; set; } = new List<Attribute>();

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

    public virtual Contact Contact { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<DiveLocation> DiveLocations { get; set; } = new List<DiveLocation>();

    public virtual ICollection<DivePlan> DivePlans { get; set; } = new List<DivePlan>();

    public virtual ICollection<DiveSite> DiveSites { get; set; } = new List<DiveSite>();

    public virtual ICollection<DiveType> DiveTypes { get; set; } = new List<DiveType>();

    public virtual ICollection<Dife> Dives { get; set; } = new List<Dife>();

    public virtual ICollection<Friend> FriendRecipientUsers { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendRequestorUsers { get; set; } = new List<Friend>();

    public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();

    public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
