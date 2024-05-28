using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Phorcys.Web.Models
{
	public class LocationViewModel
	{
		public int DiveLocationId { get; set; }	

		[DisplayName("Location Title")]
		[Required(ErrorMessage = "Location Title is required")]
		public string Title { get; set; }

		[DisplayName("Notes")]
		public string Notes { get; set; }
	}
}
