using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
	public class DiveDto
	{
		public int DiveId { get; set; }
		public int? DivePlanId { get; set; }
		public int DiveNumber { get; set; }		
		public string? Title { get; set; }
		public int? Minutes { get; set; }
		public DateTime? DescentTime { get; set; }
		public int? AvgDepth { get; set; }
		public int? MaxDepth { get; set; }
		public int? Temperature { get; set; }
		public int? AdditionalWeight { get; set; }
		public string Notes { get; set; } = null!;
		public int? UserId { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
	}
}
