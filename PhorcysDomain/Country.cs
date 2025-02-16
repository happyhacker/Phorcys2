using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Phorcys.Domain;

public partial class Country
{
	[Key]
	public required string CountryCode { get; set; }
	public required string Name { get; set; }
}
