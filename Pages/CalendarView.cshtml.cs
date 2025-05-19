using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
using ZealandZooCase.Services;

namespace ZealandZooCase.Pages
{
    public class CalendarViewModel : PageModel
    {

        ZealandDBContext _context;
        ZealandService _zealandService;
        public CalendarViewModel(ZealandDBContext context, ZealandService zealandService)
        {
            _context = context;
            _zealandService = zealandService;
        }




        public User? CurrentUser => _zealandService.SetCurrentUser();
        public string ErrorMessage { get; set; }
        public bool IsLoggedIn { get; set; }
        public List<int> RegisterEvent { get; set; }


        public List<OurEvent> UpcomingEvents { get; set; } = new();
        public DateTime CurrentMonth { get; set; }



        public void OnGet(string? month)
        {
            // Parse month parameter or use current month
            if (!string.IsNullOrEmpty(month) && DateTime.TryParseExact(month, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out var parsedMonth))
            {
                CurrentMonth = parsedMonth;
            }
            else
            {
                CurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }

            var firstDay = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            UpcomingEvents = _context.AllOurEvents
                .Where(e => e.EventDate.Date >= firstDay && e.EventDate.Date <= lastDay)
                .ToList();
            RegisterEvent = new List<int>();



            if (CurrentUser != null)
            {
                IsLoggedIn = true;
            }
            else
            {
                IsLoggedIn = false;
            }

            if (IsLoggedIn)
            {
                RegisterEvent = _context.AllEventSignups.Where(s => s.UserId == CurrentUser.UserId).Select(s => s.EventId).ToList();
            }
        }

        public List<OurEvent> GetEventsForDate(DateTime date) =>
            UpcomingEvents.Where(e => e.EventDate.Date == date.Date).ToList();






       










    }
}
