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
    public class IndexModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public IndexModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public IList<Address> Address { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Address = await _context.Addresses
                .Include(a => a.AddressPostalcodeNavigation).ToListAsync();
        }
    }
}
