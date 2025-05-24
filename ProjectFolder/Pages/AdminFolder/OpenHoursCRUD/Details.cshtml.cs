using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.OpenHoursCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DetailsModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public OpenHour OpenHour { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openhour = await _context.OpenHours.FirstOrDefaultAsync(m => m.OpenHoursId == id);
            if (openhour == null)
            {
                return NotFound();
            }
            else
            {
                OpenHour = openhour;
            }
            return Page();
        }
    }
}
