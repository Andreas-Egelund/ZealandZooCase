using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
using ZealandZooCase.Services;

namespace ZealandZooCase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

        }

        public User CurrentUser { get; set; }

        public IActionResult OnPostVisKalander()
        {
            return RedirectToPage("/EventDetails");
        }


        public IActionResult OnGet()
        {


            var username = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {
                // Already logged in — redirect to home
                return RedirectToPage("/HomePage");
            }

            // Otherwise show login or guest option
            return Page();
        }

        public IActionResult OnPostContinueAsGuest()
        {
            // Just skip login — do not set session
            return RedirectToPage("/HomePage");
        }


    }
}
