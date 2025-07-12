using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
    public class DiveTypeDto
    {
		public int DiveTypeId { get; set; }

		public string Title { get; set; } = null!;

		public string? Notes { get; set; }
	}
}
