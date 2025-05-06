using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
using ZealandZooCase.Services;
using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace ZealandZooCase.Pages
{
    public class HomePageModel : PageModel
    {
        public ZealandService _zealandService;

        public ZealandDBContext _context;

        public HomePageModel(ZealandService service, ZealandDBContext context)
        {
            _zealandService = service;
            _context = context;
        }

        public OpenHour? OpenHour { get; set; }
        public User ?CurrentUser { get; set; }

        public bool IsLoggedIn { get; set; }


        public List<OpenHour> OpenHours { get; set; }

        public List<AllOurEvent> AllEvents { get; set; }

        public IActionResult OnPostVisKalander()
        {
            return RedirectToPage("/CalendarView");
        }

        public void OnGet()
        {
            OpenHour = _context.OpenHours.FirstOrDefault();

            AllEvents = _context.AllOurEvents.ToList();

            CurrentUser = _zealandService.SetCurrentUser();

            if (CurrentUser != null)
            {
                IsLoggedIn = true;
            }
            else
            {
                IsLoggedIn = false;
            }


        }






    }
}
