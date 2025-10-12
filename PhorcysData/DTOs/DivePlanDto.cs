using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
	public class DivePlanDto
	{
		public int DivePlanId { get; set; }
		public int? DiveSiteId { get; set; }
		public string Title { get; set; } = null!;
		public int? Minutes { get; set; }
		public DateTime ScheduledTime { get; set; }
		public int? MaxDepth { get; set; }
		public string? Notes { get; set; }
		public int UserId { get; set; }
		public List<int> SelectedDiveTypeIds { get; set; } = new();
		public List<int> SelectedGearIds { get; set; } = new();
        public IList<TanksOnDiveDto> TanksOnDiveDtos { get; set; }
    }
}
