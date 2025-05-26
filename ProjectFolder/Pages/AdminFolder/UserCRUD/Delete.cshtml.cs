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
    public class DeleteModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;

        public DeleteModel(ZealandZooCase.Data.ZealandDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user =  _context.Users.FirstOrDefault(m => m.UserId == id);

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

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user =  _context.Users.Find(id);
            if (user != null)
            {
                User = user;
                // Remove the user from the AllEventSignups collection
                List<AllEventSignup> signups =  _context.AllEventSignups.Where(s => s.UserId == id).ToList();
                _context.AllEventSignups.RemoveRange(signups);
                _context.Users.Remove(User);
                 _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}
