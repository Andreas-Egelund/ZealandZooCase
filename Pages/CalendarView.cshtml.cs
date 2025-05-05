using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages
{
    public class CalendarViewModel : PageModel
    {

        ZealandDBContext _context;
        public CalendarViewModel(ZealandDBContext context)
        {
            _context = context;
        }






        public List<AllOurEvent> UpcomingEvents { get; set; } = new();
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
        }

        public List<AllOurEvent> GetEventsForDate(DateTime date) =>
            UpcomingEvents.Where(e => e.EventDate.Date == date.Date).ToList();





    }
}
