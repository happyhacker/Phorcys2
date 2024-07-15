using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Domain
{
	public class vwMyCertification
	{
		public int DiverCertificationId { get; set; }
		public string Title { get; set; }
		public int DiverId { get; set; }
		public int UserId { get; set; }
		public string? Agency {  get; set; }
		public DateTime? Certified { get; set; }
		public string? CertificationNum { get; set; }
		public string? DiverFirstname { get; set; }
		public string? DiverLastName { get; set; }
		public string? InstructorFirstName { get; set; }
		public string? InstructorLastName { get; set; }
		public string? Notes { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }


	}
}
