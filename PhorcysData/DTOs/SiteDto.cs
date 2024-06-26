using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
	public class SiteDto
	{
		public int DiveSiteId { get; set; }
		
		public int UserId { get; set; }

		public int LocationSelectedId { get; set; }

		public int? DiveLocationId { get; set; }

		public string LocationTitle { get; set; }

		public string Title { get; set; }

		public bool IsFreshWater { get; set; }

		public string? GeoCode { get; set; }


		public int? MaxDepth { get; set; }

		public string? Notes { get; set; }
	}
}
