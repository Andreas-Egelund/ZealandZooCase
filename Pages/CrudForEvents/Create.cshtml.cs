using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.CrudForEvents
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
        ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            return Page();
        }

        [BindProperty]
        public AllOurEvent AllOurEvent { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AllOurEvents.Add(AllOurEvent);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
