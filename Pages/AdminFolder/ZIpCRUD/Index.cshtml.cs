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
    public class IndexModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public IndexModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public IList<ZipCode> ZipCode { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ZipCode = await _context.ZipCodes.ToListAsync();
        }
    }
}
