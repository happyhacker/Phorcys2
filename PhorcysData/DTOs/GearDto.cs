using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
	public class GearDto
	{
		public int GearId { get; set; }
		public string Title { get; set; } = null!;
		public decimal? RetailPrice { get; set; }
		public decimal? Paid { get; set; }
		public string? Sn { get; set; }
		public DateTime? Acquired { get; set; }
		public DateTime? NoLongerUse { get; set; }
		public double? Weight { get; set; }
		public string? Notes { get; set; }
		//Tank info
		public int? Volume { get; set; }
		public int? WorkingPressure { get; set; }
		public byte? ManufacturedMonth { get; set; }
		public byte? ManufacturedYear { get; set; }

	}
}
