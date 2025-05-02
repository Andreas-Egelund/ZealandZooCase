using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.CrudForEvents
{
    public class IndexModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public IndexModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public IList<AllOurEvent> AllOurEvent { get;set; } = default!;

        public async Task OnGetAsync()
        {
            AllOurEvent = await _context.AllOurEvents
                .Include(a => a.Address).ToListAsync();
        }
    }
}
