namespace Phorcys.Web.Models
{
	public class ContactViewModel
	{
		public int ContactId { get; set; }
		public string LoggedIn { get; set; }
		public string UserName { get; set; }
		public string Company { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Address1 { get; set; } = null!;
		public string Address2 { get; set; } = null!;
		public string City { get; set; } = null!;
		public string State { get; set; } = null!;
		public string PostalCode { get; set; } = null!;
		public string? CountryCode { get; set; }
		public string Email { get; set; } = null!;
		public string CellPhone { get; set; } = null!;
		public string HomePhone { get; set; } = null!;
		public string WorkPhone { get; set; } = null!;
		public DateTime? Birthday { get; set; }
		public string Gender { get; set; } = null!;
		public string? Notes { get; set; }
		public int UserId { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
	}
}

