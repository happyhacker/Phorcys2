using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
    public class MyCertificationDto
    {
        public int DiverCertificationId { get; set; }
        public int DiverId { get; set; }
        public int CertificationId { get; set; }
        public int? InstructorId { get; set; }       
        public DateTime? Certified { get; set; }
        public string CertificationNum { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
