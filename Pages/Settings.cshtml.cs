using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBrief24.Auth;
using OnlineBrief24.Helpers;
using Renci.SshNet.Common;

namespace OnlineBrief24.Pages
{
	public class SettingsModel : PageModel
	{
		public string? SuccessMessage;
		public string? ErrorMessage;

		[BindProperty]
		[Required]
		public string Host { get; set; }
		[BindProperty]
		[Required]
		public string UploadPath { get; set; }
		[BindProperty]
		[Required]
		public string Mail { get; set; }
		[BindProperty]
		[Required]
		public string Password { get; set; }

		public void OnGet()
		{

		}

		public async Task<IActionResult> OnPost()
		{
			return Page();
		}

		public async Task<IActionResult> Test()
		{
			try
			{
				Sftp sftp = new(Mail, Password);
				if (await sftp.Connect())
				{
					SuccessMessage = "Verbindung erfolgreich.";
					return Page();
				}
				return Page();
			}
			catch (SshAuthenticationException)
			{
				ErrorMessage = "Fehler bei der Authentifizierung am Host.";
				return Page();
			}
			catch (SshConnectionException)
			{
				ErrorMessage = "Fehler bei der Verbindung zum Host.";
				return Page();
			}
		}

	}
}

