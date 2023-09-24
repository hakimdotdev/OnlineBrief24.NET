using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBrief24.Helpers;
using OnlineBrief24.Auth;

namespace OnlineBrief24.Pages
{
	[AllowAnonymous]

	public class LoginModel : PageModel
	{

		[BindProperty]
		[Required]
		[DisplayName("Username")]
		public string Username { get; set; }

		[BindProperty]
		[Required]
		[DisplayName("Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public string? ErrorMessage { get; set; }
		public LoginModel()
		{

		}
		public IActionResult OnGet(ClaimsPrincipal username)
		{
			username = HttpContext.User;
			if (username != null && username.Identity != null && username.Identity.IsAuthenticated)
			{
				return RedirectToPage("Index");
			}
			return Page();
		}

		public async Task<IActionResult> OnPost(LdapAuthenticator authenticator)
		{
			bool authenticated = authenticator.TryAuthenticateUser(Username, Password);
			Password = string.Empty;
			if (!authenticated)
			{
				ErrorMessage = "Authentifizierung fehlgeschlagen. Haben Sie einen LDAP-Server spezifiziert?";
				return Page();
			}
			var cl = new List<Claim>
				{
					new("username", Username),
					new("role", "User")
				};

			var ci = new ClaimsIdentity(cl, "ldap", "username", "role");
			var cp = new ClaimsPrincipal(ci);

			await HttpContext.SignInAsync(cp);

			return RedirectToPage("Index");
		}

	}
}
