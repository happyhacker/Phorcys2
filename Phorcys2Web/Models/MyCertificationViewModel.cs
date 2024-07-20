using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Phorcys.Web.Models
{
	public class MyCertificationViewModel
	{
		public int DiverCertificationId { get; set; }
		
		[Required]
		public int DiveAgencyId { get; set; }
		public int DiverId { get; set; }
		
		[Required]
		public int CertificationId { get; set; }

		[Required]
		public int? InstructorId { get; set; }		
		public DateTime? Certified { get; set; }
		public string CertificationNum { get; set; }
		public string Notes { get; set; }
		public DateTime Created { get; set; }

		public IList<SelectListItem> DiveAgencyListItems { get; set; }
		public IList<SelectListItem> CertificationListItems { get; set; }
		public IList<SelectListItem> InstructorListItems { get; set; }
		public SelectList InstructorSelectList { get; set; }

	}
}
