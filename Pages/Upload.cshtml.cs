using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBrief24.Models;
using File = OnlineBrief24.Models.File;
using Microsoft.AspNetCore.Authorization;
using OnlineBrief24.Helpers;
using System.Reflection.Metadata;
using Renci.SshNet.Messages.Authentication;
using Microsoft.Extensions.Hosting.Internal;
using Renci.SshNet.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OnlineBrief24.Pages
{
    public class UploadModel : PageModel
    {
		private OnlineBrief24Context _context;

		private readonly ILogger<UploadModel> _logger;
		public string? SuccessMessage;
		public string? ErrorMessage;

		public UploadModel(OnlineBrief24Context context, ILogger<UploadModel> logger) 
		{
			_logger = logger;
			_context = context;
		}
		[BindProperty]
		[Required]
		public string Account { get; set; }
		[BindProperty]
		[Required]
		public bool Colored { get; set; }
		[BindProperty]
		[Required]
		public bool Mode { get; set; }
		[BindProperty]
		[Required]
		public bool Envelope { get; set; }
		[BindProperty]
		[Required]
		public int ShippingZone { get; set; }
		[BindProperty]
		[Required]
		public int RegisteredMail { get; set; }
		[BindProperty]
		[Required]
		public int PaymentSlip { get; set; }
		[BindProperty]
		public bool SaveToDatabase { get; set; }
		[BindProperty]
		public List<IFormFile> Upload { get; set; }


		public void OnGet()
        {

        }
		[HttpPost]
		//[Authorize]
		[DisableRequestSizeLimit]
		[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]

		public async Task<IActionResult> OnPost() 
        {
			if (Upload != null && Upload.Any())
			{
				_logger.LogInformation("[{dt}] Recieved files, starting to create objects.", DateTime.Now);
				var p = new Parameters()
				{
					Colored = Colored,
					Envelope = Envelope,
					ShippingZone = ShippingZone,
					RegisteredMail = RegisteredMail,
					PaymentSlip = PaymentSlip,
					Id = Guid.NewGuid(),
					Mode = Mode
				};
				_context.Add(p);
				_logger.LogInformation("[{dt}] Added Parameters to Context - Id: {id}", DateTime.Now, p.Id);

				var dispatch = new Dispatches()
				{
					Id = Guid.NewGuid(),
					StartDate = DateTime.Now,
					Account = Account,
					Parameters = p
				};
				_context.Add(dispatch);
				_logger.LogInformation("[{dt}] Added Dispatch to Context - Id: {id} - StartDate: {startDate}", DateTime.Now, dispatch.Id, dispatch.StartDate);
				foreach (var f in Upload)
				{
					byte[] doc = null;
					if (SaveToDatabase)
					{
						doc = FileHelpers.ToByteArray(f).Result;
					}

					var file = new File
					{
						Name = f.Name,
						Dispatch = dispatch,
						Parameters = p,
						Document = doc,
						FileName = $"{p.GetFileNamePart()}-{f.Name}#{dispatch.Account}#.pdf",
						Id = Guid.NewGuid(),
					};
					_context.Add(file);
					_logger.LogInformation("[{dt}] Added File to Context - Name: {fileName}", DateTime.Now, file.FileName);

				}
				_logger.LogInformation("[{dt}] Starting upload.", DateTime.Now);
				//TODO: CREDS
				Sftp sftp = new("abc@def.ghj", "MYSECUREPASSXD");
				try
				{
					await sftp.Upload(Upload);
					_logger.LogInformation("[{dt}] Upload Successful", DateTime.Now);
					_context.SaveChanges();
					_logger.LogInformation("[{dt}] Saved Changes to Database", DateTime.Now);
					SuccessMessage = "Erfolgreich hochgeladen und gespeichert.";
					return Page();
				}
				catch (SshAuthenticationException)
				{
					_logger.LogInformation("[{dt}] Couldn't upload, authentication Failed.", DateTime.Now);
					_context.Dispose();
					ErrorMessage = "Fehler bei der Authentifizierung am Host.";
					return Page();
				}
				catch (SshConnectionException)
				{
					_logger.LogInformation("[{dt}] Couldn't upload, connection was terminated.", DateTime.Now);
					_context.Dispose();
					ErrorMessage = "Fehler bei der Verbindung zum Host.";
					return Page();
				}
			}
			ErrorMessage = "Keine Dateien ausgewählt.";
			return Page();
		}
	}
}
