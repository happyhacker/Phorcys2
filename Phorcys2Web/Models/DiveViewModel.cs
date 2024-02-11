using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phorcys.Web.Models
{
    public class DiveViewModel
    {
        public int DiveId { get; set; }

        public string DivePlanTitle { get; set; }

        public IList<SelectListItem> DivePlanList { get; set; }
        public int DivePlanSelectedId { get; set; }

		public string DiveSite { get; set; }

        public string UserName { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]
		public int? Minutes { get; set; }

        public DateTime? DescentTime { get; set; }

        public int? AvgDepth { get; set; }

        [DisplayName("Max Depth")]
        [Range(0,1200, ErrorMessage = "You didn't really go this deep, right?")]
        public int? MaxDepth { get; set; }

        public int? Temperature { get; set; }

        public int? AdditionalWeight { get; set; }

        public string Notes { get; set; } = null!;

        public int DiveNumber { get; set; }

        public int? UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

    }
}
