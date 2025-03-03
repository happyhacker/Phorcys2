using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Data.DTOs
{
    public class ContactDto
    {
		public int ContactId { get; set; }
        public bool IsInstructor { get; set; }
        public bool IsDiver { get; set; }
        public bool IsManufacturer { get; set; }
        public bool IsAgency { get; set; }
        public bool IsDiveShop { get; set; }
        public string Company { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Address1 { get; set; } = null!;
		public string Address2 { get; set; } = null!;
		public string City { get; set; } = null!;
		public string State { get; set; } = null!;
		public string PostalCode { get; set; } = null!;
		public string? CountryCode { get; set; }
		public string? Email { get; set; } = null!;
		public string? CellPhone { get; set; } = null!;
		public string? HomePhone { get; set; } = null!;
		public string? WorkPhone { get; set; } = null!;
		public DateTime? Birthday { get; set; }
		public string? Gender { get; set; } = null!;
		public string? Notes { get; set; }
		public int UserId { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
	}
}

