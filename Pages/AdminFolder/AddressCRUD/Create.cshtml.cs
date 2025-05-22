using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.AddressCRUD
{
    public class CreateModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public CreateModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AddressPostalcode"] = new SelectList(_context.ZipCodes.OrderBy(z => z.Postalcode), "Postalcode", "Postalcode");
            return Page();
        }

        [BindProperty]
        public Address Address { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Addresses.Add(Address);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
