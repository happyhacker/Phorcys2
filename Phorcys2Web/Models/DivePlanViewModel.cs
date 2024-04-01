using Microsoft.AspNetCore.Mvc.Rendering;
using Phorcys.Domain;
using System.ComponentModel;

namespace Phorcys.Web.Models
{
	public class DivePlanViewModel
	{
		public int DivePlanId { get; set; }
		public int? DiveSiteId { get; set; }
		public int? DiveSiteSelectedId { get; set; }
		public IList<SelectListItem> DiveSiteList { get; set; }

		[DisplayName("Title")]
		public string Title { get; set; } = null!;
		[DisplayName("Minutes")]
		public int? Minutes { get; set; }
		[DisplayName("Scheduled Time")]
		public DateTime ScheduledTime { get; set; }
		[DisplayName("Max Depth")]
		public int? MaxDepth { get; set; }
		[DisplayName("Notes")]
		public string? Notes { get; set; }
		public int UserId { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
		public virtual DiveSite? DiveSite { get; set; }

		//public virtual ICollection<TanksOnDive> TanksOnDives { get; set; } = new List<TanksOnDive>();

		//public required User User { get; set; }
	}
}
