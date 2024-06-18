using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
	public class LocationDto
	{
		public int DiveLocationId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string? Notes { get; set; }
	}
}
