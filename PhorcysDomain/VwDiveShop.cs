using System;
using System.Collections.Generic;

namespace Phorcys.Domain;

public partial class VwDiveShop
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

    public string Notes { get; set; } = null!;

    public int UserId { get; set; }

    public string ShopNotes { get; set; } = null!;
}
