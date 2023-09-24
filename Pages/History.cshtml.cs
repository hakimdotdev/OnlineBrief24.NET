using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBrief24.Models;

namespace OnlineBrief24.Pages
{
    public class HistoryModel : PageModel
    {
        public ICollection<Dispatches> _dispatches { get; set; }
		private readonly OnlineBrief24Context _context;

		public HistoryModel(OnlineBrief24Context context)
        {
            _context = context;
            _dispatches = _context.Dispatches.ToList();
        }
        public void OnGet()
        {
        }

    }
}
