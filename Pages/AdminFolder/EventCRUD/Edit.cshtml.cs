using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.EventCRUD
{
    public class EditModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public EditModel(ZealandZooCase.Data.ZealandDBContext context)
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

            var ourevent =  await _context.AllOurEvents.Include(o => o.Address).ThenInclude(a => a.AddressPostalcodeNavigation).FirstOrDefaultAsync(m => m.EventId == id);
            if (ourevent == null)
            {
                return NotFound();
            }
            OurEvent = ourevent;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(OurEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OurEventExists(OurEvent.EventId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OurEventExists(int id)
        {
            return _context.AllOurEvents.Any(e => e.EventId == id);
        }
    }
}
