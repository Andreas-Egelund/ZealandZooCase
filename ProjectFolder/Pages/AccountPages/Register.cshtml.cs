using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
namespace ZealandZooCase.Pages.AccountPages
{
    public class RegisterModel : PageModel
    {
        private readonly ZealandDBContext _context;

        public RegisterModel(ZealandDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }


        [BindProperty]
        public string Email { get; set; } 

        [BindProperty]
        public bool WantsNewsletter { get; set; } 

        public string ErrorMessage { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            try
            {
                if (_context.Users.Any(u => u.UserName.ToLower() == Username.ToLower()))
                {
                    ErrorMessage = "Username already exists.";
                    return Page();
                }
                else if (_context.Users.Any(u => u.UserEmail.ToLower() == Email.ToLower()))
                {
                    ErrorMessage = "Email is Already connected to another account";
                    return Page();
                }

                var user = new User { UserName = Username, UserPassword = Password, UserEmail = Email };
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToPage("/AccountPages/LoginPage");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
               
                return Page();
            }
        }
    }
}
