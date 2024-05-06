using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class InsurancePolicy
{
    public int InsurancePolicyId { get; set; }

    public int ContactId { get; set; }

    public DateOnly? StartOfTerm { get; set; }

    public DateOnly? EndOfTerm { get; set; }

    public decimal? Deductible { get; set; }

    public decimal? ValueAmount { get; set; }

    public string Notes { get; set; }

    public virtual Contact Contact { get; set; }

    public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();
}
