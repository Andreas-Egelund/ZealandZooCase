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
        // Henter ZealandService og ZealandDBContext for at kunne bruge dem og vores metoder i services
        public ZealandService _zealandService;
        public ZealandDBContext _context;

        public ProfileSiteModel(ZealandService zealandService, ZealandDBContext context)
        {
            // Initialiserer ZealandService og ZealandDBContext
            _zealandService = zealandService;
            _context = context;
        }


        // Henter den nuværende brugerens login information
        public User? CurrentUser => _zealandService.SetCurrentUser();



        // orderbydescending for at sortere efter signupdato 
        public IQueryable<AllEventSignup> TilmeldteEvents => _context.AllEventSignups.Where(e => e.UserId == CurrentUser.UserId).Include(e => e.Event).OrderByDescending(e => e.SignupDate);


        // Her definere linq query
        public IQueryable<AllEventSignup> AllPaidEvents => _context.AllEventSignups.Where(e => e.UserId == CurrentUser.UserId && e.Event.EventTicketPrice > 0).Include(e => e.Event).OrderByDescending(e => e.SignupDate);


        public void OnGet()
        {
            TilmeldteEvents.ToList(); //Her exekveres query til at hente alle tilmeldte events
            AllPaidEvents.ToList(); //Her exekveres query til at hente alle betalte events

        }



        public IActionResult OnPostTilmelding()
        {
            // Opdaterer nyhedsbrev property til true.
            CurrentUser.UserNewsletter = true;
            _context.SaveChanges();
            return Page();
        }

        public IActionResult OnPostAfmelding()
        {
            // Opdaterer nyhedsbrev property til false.
            CurrentUser.UserNewsletter = false;
            _context.SaveChanges();
            return Page();
        }



    }
}
