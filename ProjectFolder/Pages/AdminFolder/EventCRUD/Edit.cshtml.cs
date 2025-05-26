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

namespace ZealandZooCase.Pages.AdminFolder.EventCrud
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

        [BindProperty]
        public string StreetFromForm { get; set; }

        [BindProperty]
        public string PostalCode { get; set; }

        public SelectList PostalCodeList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            OurEvent = await _context.AllOurEvents
                .Include(e => e.Address)
                .ThenInclude(a => a.AddressPostalcodeNavigation)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (OurEvent == null) return NotFound();

            // Pre-fill address form values
            StreetFromForm = OurEvent.Address?.Street;
            PostalCode = OurEvent.Address?.AddressPostalcode;

            // Fill postal code dropdown (use Postalcode as both value and display)
            PostalCodeList = new SelectList(await _context.ZipCodes.OrderBy(z => z.Postalcode).ToListAsync(), "Postalcode", "Postalcode");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            var existingEvent = await _context.AllOurEvents
                .Include(e => e.Address)
                .FirstOrDefaultAsync(e => e.EventId == OurEvent.EventId);

            if (existingEvent == null) return NotFound();

            // Update event core fields
            existingEvent.EventName = OurEvent.EventName;
            existingEvent.EventDescription = OurEvent.EventDescription;
            existingEvent.EventDate = OurEvent.EventDate;
            existingEvent.EventMaxAttendance = OurEvent.EventMaxAttendance;
            existingEvent.EventTicketPrice = OurEvent.EventTicketPrice;
            existingEvent.EventImageName = OurEvent.EventImageName;

            // Lookup ZipCode info
            var selectedZip = await _context.ZipCodes.FirstOrDefaultAsync(z => z.Postalcode == PostalCode);
            if (selectedZip == null)
            {
                ModelState.AddModelError("PostalCode", "Invalid postal code selected.");
                PostalCodeList = new SelectList(await _context.ZipCodes.ToListAsync(), "Postalcode", "Postalcode");
                return Page();
            }

            // Update address

            var newAddress = new Address()
            {
                Street = StreetFromForm,
                AddressPostalcode = selectedZip.Postalcode
            };

            existingEvent.Address = newAddress;
         

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        private bool OurEventExists(int id)
        {
            return _context.AllOurEvents.Any(e => e.EventId == id);
        }
    }


}
