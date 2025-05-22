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
using ZealandZooCase.Services;

namespace ZealandZooCase.Pages.AdminFolder.EventCrud
{
    public class CreateModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;
        private readonly ZealandService _service;

        public CreateModel(ZealandZooCase.Data.ZealandDBContext context, ZealandService service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult OnGet()
        {
            // Fill postal code dropdown (use Postalcode as both value and display)
            PostalCodeList = new SelectList( _context.ZipCodes.ToList(), "Postalcode", "Postalcode");


            return Page();
        }

        [BindProperty]
        public OurEvent OurEvent { get; set; } = default!;


        [BindProperty]
        public string StreetFromForm { get; set; }

        [BindProperty]
        public string PostalCodeFromForm { get; set; }


        public SelectList PostalCodeList { get; set; }




        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}



            // Lookup ZipCode info
            var selectedZip = await _context.ZipCodes.FirstOrDefaultAsync(z => z.Postalcode == PostalCodeFromForm);
            if (selectedZip == null)
            {
                ModelState.AddModelError("PostalCode", "Invalid postal code selected.");
                PostalCodeList = new SelectList(await _context.ZipCodes.OrderBy(z => z.Postalcode).ToListAsync(), "Postalcode", "Postalcode");
                return Page();
            }


            if(_context.Addresses.Any(a => a.Street == StreetFromForm && a.AddressPostalcode == selectedZip.Postalcode))
            {
                var existingAddress = _context.Addresses.FirstOrDefault(a => a.Street == StreetFromForm && a.AddressPostalcode == selectedZip.Postalcode);
                OurEvent.Address = existingAddress;
            }
            else
            {

                var newAddress = new Address()
                {
                    Street = StreetFromForm,
                    AddressPostalcode = selectedZip.Postalcode
                };

                OurEvent.Address = newAddress;


            }

            var OurCustomer = _context.Users.Where(u => u.UserNewsletter == true).ToList();

            var AddEvent = _context.AllOurEvents.Add(OurEvent);
            await _context.SaveChangesAsync();
            foreach (var item in OurCustomer)
            {
                _service.SendMail(item, OurEvent);
            }

            return RedirectToPage("./Index");
        }
    }
}
