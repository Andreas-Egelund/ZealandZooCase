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

        public User? CurretUser { get; set; }
        public void OnGet()
        {
            CurretUser =  _zealandService.SetCurrentUser();

        }

        public List<AllEventSignup> TilmeldteEvents => _context.AllEventSignups.Where(e => e.UserId == CurretUser.UserId).Include(e => e.Event).ToList();

        public IActionResult OnPostTilmelding()
        {
            CurretUser = _zealandService.SetCurrentUser();
            CurretUser.UserNewsletter = true;
            _context.SaveChanges();
            return Page();
        }

        public IActionResult OnPostAfmelding()
        {
            CurretUser = _zealandService.SetCurrentUser();
            CurretUser.UserNewsletter = false;
            _context.SaveChanges();
            return Page();
        }



    }
}
