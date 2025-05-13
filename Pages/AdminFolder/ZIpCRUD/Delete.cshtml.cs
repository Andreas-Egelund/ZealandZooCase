using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.ZIpCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DeleteModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ZipCode ZipCode { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zipcode = await _context.ZipCodes.FirstOrDefaultAsync(m => m.Postalcode == id);

            if (zipcode == null)
            {
                return NotFound();
            }
            else
            {
                ZipCode = zipcode;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zipcode = await _context.ZipCodes.FindAsync(id);
            if (zipcode != null)
            {
                ZipCode = zipcode;
                _context.ZipCodes.Remove(ZipCode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
