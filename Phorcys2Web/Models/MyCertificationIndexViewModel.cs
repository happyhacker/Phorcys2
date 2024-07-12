using System.ComponentModel.DataAnnotations;

namespace Phorcys.Web.Models
{
	public class MyCertificationIndexViewModel
	{
		public int DiverCertificationId { get; set; }

		public int DiverId { get; set; }		
		
		[Required]
		public int DiveAgencyId { get; set; }

		public int InstructorId { get; set; }		

		[Required]
		public int CertificationId { get; set; }

		public DateTime? Certified { get; set; }

		public string CertificationNumber { get; set; }

		public string Notes { get; set; }

		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }


	}
}
