using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Phorcys.Web.Models
{
    public class SiteViewModel
    {
        public int DiveSiteId { get; set; }

        public IList<SelectListItem> LocationList { get; set; }

        public int? DiveLocationId { get; set; }

        public string LocationTitle { get; set; }

        [DisplayName("Dive Site Title")]
        [Required]
        public string Title { get; set; } = string.Empty;

        public bool IsFreshWater { get; set; }

        [DisplayName("Geo Code")]
        public string? GeoCode { get; set; }

        [DisplayName("Max Depth")]
        [Range(0, 1200, ErrorMessage = "Please enter a reasonable depth you Sheck wannabe :-)")]
        public int? MaxDepth { get; set; }

        [DisplayName("Notes")]
        public string? Notes { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

    }
}
