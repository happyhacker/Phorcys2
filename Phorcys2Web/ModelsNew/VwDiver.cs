using System;
using System.Collections.Generic;

namespace Phorcys.Web.ModelsNew;

public partial class VwDiver
{
    public int DiverId { get; set; }

    public double? WorkingSacRate { get; set; }

    public double? RestingSacRate { get; set; }

    public string DiverNotes { get; set; }

    public int ContactId { get; set; }

    public string Company { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string PostalCode { get; set; }

    public string CountryCode { get; set; }

    public string Email { get; set; }

    public string CellPhone { get; set; }

    public string HomePhone { get; set; }

    public string WorkPhone { get; set; }

    public DateOnly? Birthday { get; set; }

    public string Gender { get; set; }

    public string Notes { get; set; }

    public int UserId { get; set; }
}
