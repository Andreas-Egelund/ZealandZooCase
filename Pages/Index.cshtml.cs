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
        private readonly ZealandDBContext _dbContext;


        public IndexModel(ILogger<IndexModel> logger, ZealandDBContext context)
        {
            _logger = logger;
            _dbContext = context;

        }

        

        public OpenHour? OpenHour { get; set; }


      

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

            // Fetch the first OpenHour record from the database
            // Fetch the first OpenHour record from the database
            OpenHour = _dbContext.OpenHours.FirstOrDefault();





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
