using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.EventCrud
{
    public class DetailsModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DetailsModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public OurEvent OurEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ourevent = await _context.AllOurEvents.Include(e => e.Address).ThenInclude(a => a.AddressPostalcodeNavigation).FirstOrDefaultAsync(m => m.EventId == id);
            if (ourevent == null)
            {
                return NotFound();
            }
            else
            {
                OurEvent = ourevent;
            }
            return Page();
        }
    }
}
