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
            try
            {
                // Opdaterer nyhedsbrev property til true.
                CurrentUser.UserNewsletter = true;
                _context.SaveChanges();
                return Page();
            }
            catch (Exception ex)
            {
                // Håndterer fejl
                ModelState.AddModelError(string.Empty, "There was an error while subscribing to the newsletter: " + ex.Message);
                return Page();
            }
        }

        public IActionResult OnPostAfmelding()
        {
            try
            {

                // Opdaterer nyhedsbrev property til false.
                CurrentUser.UserNewsletter = false;
                _context.SaveChanges();
                return Page();
            }
            catch(Exception ex)
            {
                // Håndterer fejl
                ModelState.AddModelError(string.Empty, "There was an error while unsubscribing from the newsletter: " + ex.Message);

            }
            return Page();

        }


        public IActionResult OnPostLogout()
        {
            try
            {
                // Logger brugeren ud og sletter sessionen
                HttpContext.Session.Clear();
                return RedirectToPage("/Index");
            }
            catch (Exception ex) 
            {
                // Håndterer fejl
                ModelState.AddModelError(string.Empty, "There was an error while logging out profile: " + ex.Message);
            }
            return Page();
        }




        public IActionResult OnPostUnsubscribeFromEvent(int eventId)
        {
            try
            { 
            // Henter den tilmeldte event fra databasen
            var signup = _context.AllEventSignups.FirstOrDefault(e => e.UserId == CurrentUser.UserId && e.EventId == eventId);
            if (signup != null)
            {
                // Sletter den tilmeldte event
                _context.AllEventSignups.Remove(signup);
                _context.SaveChanges();
            }
            return Page();
            }
            catch (Exception ex)
            {
                // Håndterer fejl
                ModelState.AddModelError(string.Empty, "There was an error while unsubscribing from event  : " + ex.Message);
            }
            
            return Page();
        }




    }
}
