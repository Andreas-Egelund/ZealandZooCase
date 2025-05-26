using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.AddressCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DetailsModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public Address Address { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.Include(a => a.AddressPostalcodeNavigation).FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            else
            {
                Address = address;
            }
            return Page();
        }
    }
}
