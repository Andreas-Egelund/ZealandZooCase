using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.UserCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DetailsModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        public User User { get; set; } = default!;

        public List<AllEventSignup> TilmeldteEvents => _context.AllEventSignups.Where(e => e.UserId == User.UserId).Include(e => e.Event).OrderByDescending(e => e.SignupDate).ToList();




        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
