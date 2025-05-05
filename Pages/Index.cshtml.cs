using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private ZealandDBContext ZealandDBContext;
        public IndexModel(ILogger<IndexModel> logger, ZealandDBContext context)
        {
            _logger = logger;
            ZealandDBContext = context;
        }

        public List<AllOurEvent> ViewAllOurEvents { get; set; }

        public void OnGet()
        {

            ViewAllOurEvents = ZealandDBContext.AllOurEvents.ToList();
        }


        public IActionResult OnPostVisKalander()
        {
            return RedirectToPage("/EventDetails");
        }
    }
}
