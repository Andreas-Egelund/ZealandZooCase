using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.CrudForEvents
{
    public class DetailsModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DetailsModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public AllOurEvent AllOurEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allourevent = await _context.AllOurEvents.FirstOrDefaultAsync(m => m.EventId == id);
            if (allourevent == null)
            {
                return NotFound();
            }
            else
            {
                AllOurEvent = allourevent;
            }
            return Page();
        }
    }
}
