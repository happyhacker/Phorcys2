using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phorcys.Web.Models
{
    public class MapViewModel
    {
        [Column(TypeName = "decimal(9,6)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal? Longitude { get; set; }
        public int DiveSiteId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Water { get; set; } = string.Empty;
        public int? MaxDepth { get; set; }
        [DisplayName("Notes")]
        public string? Notes { get; set; }
    }
}
