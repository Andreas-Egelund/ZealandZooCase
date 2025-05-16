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
    public class DeleteModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DeleteModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OurEvent OurEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ourevent = await _context.AllOurEvents.FirstOrDefaultAsync(m => m.EventId == id);





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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ourevent = await _context.AllOurEvents.Include(e => e.Address).ThenInclude(a => a.AddressPostalcodeNavigation).FirstOrDefaultAsync(m => m.EventId == id);


            if (ourevent != null)
            {
                OurEvent = ourevent;
                // Remove the event signups associated with this event
                var eventSignups = await _context.AllEventSignups.Where(es => es.EventId == OurEvent.EventId).ToListAsync();
                _context.AllEventSignups.RemoveRange(eventSignups);

                await _context.SaveChangesAsync();
            
                _context.AllOurEvents.Remove(OurEvent);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
