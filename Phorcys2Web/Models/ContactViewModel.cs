using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Phorcys.Web.Models
{
	public class ContactViewModel
	{
		public int ContactId { get; set; }
		public string SelectedCountryCode { get; set; }
		public bool IsInstructor { get; set; }
		public bool IsDiver {  get; set; }
		public bool IsManufacturer { get; set; }
		public bool IsAgency { get; set; }
		public bool IsDiveShop { get; set; }
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

		[EmailAddress(ErrorMessage = "Invalid email format")] 
		public string Email { get; set; } = null!;

        [RegularExpression(@"^\+?[1-9]\d{0,2}[\s\-]?\(?\d+\)?([\s\-]?\d+)*$",
            ErrorMessage = "Enter a valid international phone number.")]
        public string CellPhone { get; set; }

		[Phone(ErrorMessage = "Invalid phone number format")]
		public string HomePhone { get; set; }

		[Phone(ErrorMessage = "Invalid phone number format")]
		public string WorkPhone { get; set; }

		public DateTime? Birthday { get; set; }
		public string Gender { get; set; }
		public string? Notes { get; set; }
		public int UserId { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
		public IList<SelectListItem> CountryList { get; set; }
	}
}

