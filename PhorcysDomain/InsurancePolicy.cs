using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class InsurancePolicy
{
    public int InsurancePolicyId { get; set; }

    public int ContactId { get; set; }

    public DateTime? StartOfTerm { get; set; }

    public DateTime? EndOfTerm { get; set; }

    public decimal? Deductible { get; set; }

    public decimal? ValueAmount { get; set; }

    public string? Notes { get; set; }

    public virtual Contact Contact { get; set; } = null!;

    public virtual ICollection<Gear> Gears { get; set; } = new List<Gear>();
}
