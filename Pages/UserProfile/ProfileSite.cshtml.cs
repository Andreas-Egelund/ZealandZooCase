using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
using ZealandZooCase.Services;

namespace ZealandZooCase.Pages.UserProfile
{
    public class ProfileSiteModel : PageModel
    {
        public ZealandService _zealandService;
        public ZealandDBContext _context;

        public ProfileSiteModel(ZealandService zealandService, ZealandDBContext context)
        {
            _zealandService = zealandService;
            _context = context;
        }

        public User? CurrentUser { get; set; }

        public List<AllEventSignup> TilmeldteEvents { get; set; }



        public List<AllEventSignup> AllPaidEvents { get; set; }



        public void OnGet()
        {
            CurrentUser =  _zealandService.SetCurrentUser();

            TilmeldteEvents = _context.AllEventSignups.Where(e => e.UserId == CurrentUser.UserId).Include(e => e.Event).OrderByDescending(e => e.SignupDate).ToList();

            AllPaidEvents = _context.AllEventSignups.Where(e => e.UserId == CurrentUser.UserId && e.Event.EventTicketPrice > 0).Include(e => e.Event).OrderByDescending(e => e.SignupDate).ToList();

        }



        public IActionResult OnPostTilmelding()
        {
            CurrentUser = _zealandService.SetCurrentUser();
            CurrentUser.UserNewsletter = true;
            _context.SaveChanges();
            return Page();
        }

        public IActionResult OnPostAfmelding()
        {
            CurrentUser = _zealandService.SetCurrentUser();
            CurrentUser.UserNewsletter = false;
            _context.SaveChanges();
            return Page();
        }



    }
}
