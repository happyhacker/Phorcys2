using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace Phorcys.Web.Models
{
    public class SiteViewModel
    {
		//A hack to pass in the logged in user
        public string LoggedIn { get; set; }

		public int DiveSiteId { get; set; }

		public int UserId { get; set; }

		public string UserName { get; set; }

        public IList<SelectListItem> LocationList { get; set; }

        public int LocationSelectedId { get; set; }

        public int? DiveLocationId { get; set; }

        public string LocationTitle { get; set; }

        [DisplayName("Dive Site Title")]
        [Required]
        public string Title { get; set; } = string.Empty;

        public bool IsFreshWater { get; set; }

        [DisplayName("Geo Code")]
		[MaxLength(30)]
		public string? GeoCode { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        [DisplayName("Latitude")]
		[Range(-90, 90)]
		public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        [DisplayName("Latitude")]
		[Range(-180, 180)]
		public decimal? Longitude { get; set; }

		public string SiteMapUrl => $"http://maps.google.com/maps?q={GeoCode}&ll={GeoCode}&z=14\\";

		[DisplayName("Max Depth")]
        [Range(0, 1200, ErrorMessage = "Please enter a reasonable depth you Sheck wannabe :-)")]
        public int? MaxDepth { get; set; }

		public DateTime Created { get; set; }

		public DateTime LastModified { get; set; }

		[DisplayName("Notes")]
        public string? Notes { get; set; }

		public virtual string Url4Map(string geoCode)
		{
			var retVal = new StringBuilder("");

			if (geoCode != null && geoCode.Trim().Length > 0)
			{
				retVal.Append("<a href=\"http://maps.google.com/maps?q=");
				retVal.Append(geoCode.Trim());
				//arrow is centered
				retVal.Append("&ll=");
				retVal.Append(geoCode.Trim());
				//zoom level
				retVal.Append("&z=14");
				retVal.Append("\"");
				retVal.Append(" target=\"_blank\" ");
				//retVal.Append("onclick='return ! window.open(this.href);'");
				retVal.Append(">Map</a>");
			}
			return retVal.ToString();
		}

	}
}
