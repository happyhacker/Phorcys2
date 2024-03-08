using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phorcys.Web.Models
{
    public class DivePlanIndexViewModel
    {
        public int DivePlanId { get; set; }

		[DisplayName("Title")]
		public string Title { get; set; }
 	    
		[DisplayName("Dive Site")]       
		public string DiveSite { get; set; }

		[DisplayName("User Name")]
		public string UserName { get; set; }

		[DisplayName("Minutes")] 
        public int? Minutes { get; set; }

		[DisplayName("Scheduled Time")]
		public DateTime? ScheduledTime { get; set; }
		
		[DisplayName("Max Depth")]
		public int? MaxDepth { get; set; }

		[DisplayName("Notes")]
		public string Notes { get; set; }

        [DisplayName("UserId")]
        public int? UserId { get; set; }
    }
}
