using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;

[AllowAnonymous]
public class ResetPasswordModel : PageModel
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly ILogger<ResetPasswordModel> _logger;

	public ResetPasswordModel(UserManager<IdentityUser> userManager, ILogger<ResetPasswordModel> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	[BindProperty]
	public InputModel Input { get; set; }

	public class InputModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string Token { get; set; }
	}

    public IActionResult OnGet(string code = null)
    {
        if (string.IsNullOrEmpty(code))
        {
            _logger.LogError("A reset token must be supplied for password reset.");
            return BadRequest("A reset token must be supplied for password reset.");
        }

        // Decode the token from URL-safe base64
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        Input = new InputModel
        {
            Token = decodedToken
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var user = await _userManager.FindByEmailAsync(Input.Email);
		if (user == null)
		{
			// Don't reveal that the user does not exist
			_logger.LogWarning("User not found during password reset attempt.");
			return RedirectToPage("./ResetPasswordConfirmation");
		}

		// Reset the user's password
		var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.Password);
		if (result.Succeeded)
		{
			_logger.LogInformation("User reset their password successfully.");
			return RedirectToPage("./ResetPasswordConfirmation");
		}

		foreach (var error in result.Errors)
		{
			ModelState.AddModelError(string.Empty, error.Description);
		}
		return Page();
	}
}
