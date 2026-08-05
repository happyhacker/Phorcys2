#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Phorcys.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        public string Email { get; set; }

        public string ReturnUrl { get; set; }

        public IActionResult OnGet(string email, string returnUrl = null)
        {
            if (email == null)
            {
                // No email was supplied (e.g. the page was navigated to directly) - there's
                // nothing useful to show, so send the user to the login page instead of a
                // Razor Pages root Index that doesn't exist in this MVC-based app.
                return RedirectToPage("./Login");
            }

            Email = email;
            ReturnUrl = returnUrl;

            return Page();
        }
    }
}
