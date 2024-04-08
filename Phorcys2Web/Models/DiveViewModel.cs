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

		[DisplayName("Dive Plan title")]
		public string DivePlanTitle { get; set; }
		
		[DisplayName("Title")]
		[Required]
		public string Title { get; set; }
        public IList<SelectListItem> DivePlanList { get; set; }
        public int DivePlanSelectedId { get; set; }

		[DisplayName("Dive Site")]
		public string DiveSite { get; set; }

		[DisplayName("User Name")]
		public string UserName { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]

		[DisplayName("Minutes")] 
        public int? Minutes { get; set; }

		[DisplayName("Descent Time")]
		public DateTime? DescentTime { get; set; }

		[DisplayName("Average Depth")]
		public int? AvgDepth { get; set; }

        [DisplayName("Max Depth")]
        [Range(0,1100, ErrorMessage = "You didn't really go this deep.")]
        public int? MaxDepth { get; set; }  

		[DisplayName("temperature")]
		public int? Temperature { get; set; }

		[DisplayName("Additional Weight")]
		public int? AdditionalWeight { get; set; }

		[DisplayName("Notes")]
		public string Notes { get; set; }

		[DisplayName("Dive #")]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]
		public int DiveNumber { get; set; }

        [DisplayName("UserId")]
        public int? UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

    }
}
