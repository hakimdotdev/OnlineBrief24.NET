using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineBrief24.Pages
{
    public class SettingsModel : PageModel
    {
        private string _host { get; set; }
        private string _path { get; set; }
        private string _mail { get; set; }
        private string _password { get; set;  }
        public void OnGet()
        {

        }
    }
}
