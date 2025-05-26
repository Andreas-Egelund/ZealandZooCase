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
        private readonly ZealandService _service;

        public IndexModel(ILogger<IndexModel> logger, ZealandDBContext context, ZealandService service)
        {
            _logger = logger;
            _dbContext = context;
            _service = service;

        }

        public List<OurEvent> UpcomingEvents { get; set; }

        public OpenHour? OpenHour { get; set; }


        public string Error { get; set; }



        public IActionResult OnPostVisKalander()
        {
            return RedirectToPage("/EventDetails");
        }



        public IActionResult OnGet()
        {
            try
            {


                // Retrieve the username from the session
                var user = _service.SetCurrentUser();

                // If not logged in, fetch the first OpenHour record from the database
                OpenHour = _dbContext.OpenHours.FirstOrDefault();


                // Check if the user is already logged in
                if (user != null)
                {
                    // If logged in, redirect to the home page
                    return RedirectToPage("/HomePage");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while retrieving the OpenHour record.");
                // Handle the error (e.g., show an error message)
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the OpenHour record.");
            }

            // Display the login or guest option page
            return Page();
        }





        public IActionResult OnPostContinueAsGuest()
        {
            // Just skip login — do not set session
            return RedirectToPage("/HomePage");
        }


    }
}
