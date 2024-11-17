using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Phorcys.Web.Models
{
	public class GearViewModel
	{
		public int GearId { get; set; }

		[DisplayName("Gear Name")]
		[Required]
		public string Title { get; set; } = null!;

		[DisplayName("Retail Price")]
        public decimal? RetailPrice { get; set; }

		[DisplayName("Paid")]
		public decimal? Paid { get; set; }
		public string? Sn { get; set; }
		public DateTime? Acquired { get; set; }
		public DateTime? NoLongerUse { get; set; }

		[DisplayName("Weight")]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid weight.")]
		public double? Weight { get; set; }

		[DisplayName("Tank Volume")]
		[Range(1,200, ErrorMessage = "Please enter a valid volume.")]
		public int? TankVolume { get; set; }

		[DisplayName("Working Pressure")]
		[Range(1, 5000, ErrorMessage = "Please enter a valid Pressure.")]
		public int? WorkingPressure { get; set; }
		
		[DisplayName("Manufactured Month")]
		[Range(1, 12, ErrorMessage = "Please enter a valid month.")]
		public byte? ManufacturedMonth { get; set; }

		[DisplayName("Manufactured Year")]
		[Range(0, 99, ErrorMessage = "Please enter a valid year.")]
		public byte? ManufacturedYear { get; set; }	
		
		public string? Notes { get; set; }
		public int UserId { get; set; }
	}
}
