using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Phorcys.Web.Models
{
	public class GearViewModel
	{
		public int GearId { get; set; }

		[DisplayName("Dive Plan title")]
		[Required]
		public string Title { get; set; } = null!;

		[DisplayName("Retail Price")]
		[Range(0, double.MaxValue, ErrorMessage = "Please enter a valid amount.")]
        public decimal? RetailPrice { get; set; }

		[DisplayName("Paid")]
		[Range(0, double.MaxValue, ErrorMessage = "Please enter a valid amount.")]
		public decimal? Paid { get; set; }
		public string? Sn { get; set; }
		public DateTime? Acquired { get; set; }
		public DateTime? NoLongerUse { get; set; }

		[DisplayName("Weight")]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid weight.")]
		public double? Weight { get; set; }

		public string? Notes { get; set; }
		public int UserId { get; set; }
	}
}
