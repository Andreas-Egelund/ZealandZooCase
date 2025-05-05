using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using ZealandZooCase.Data;

namespace ZealandZooCase.Pages.AccountPages
{
    public class LoginPageModel : PageModel
    {
        private readonly ZealandDBContext _context;

        public LoginPageModel(ZealandDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }



        public string ErrorMessage { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == Username && u.UserPassword == Password);

            if (user == null)
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            HttpContext.Session.SetString("Username", Username);
            return RedirectToPage("/HomePage");
        }
    }
}
