using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class Gear
{
    public int GearId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? PurchasedFromContactId { get; set; }

    public string Title { get; set; }

    public decimal? RetailPrice { get; set; }

    public decimal? Paid { get; set; }

    public string Sn { get; set; }

    public DateOnly? Acquired { get; set; }

    public DateOnly? NoLongerUse { get; set; }

    public double? Weight { get; set; }

    public string Notes { get; set; }

    public int UserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual ICollection<GearServiceEvent> GearServiceEvents { get; set; } = new List<GearServiceEvent>();

    public virtual Manufacturer Manufacturer { get; set; }

    public virtual Contact PurchasedFromContact { get; set; }

    public virtual ICollection<ServiceSchedule> ServiceSchedules { get; set; } = new List<ServiceSchedule>();

    public virtual SoldGear SoldGear { get; set; }

    public virtual Tank Tank { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<DivePlan> DivePlans { get; set; } = new List<DivePlan>();

    public virtual ICollection<Diver> Divers { get; set; } = new List<Diver>();

    public virtual ICollection<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();
}
