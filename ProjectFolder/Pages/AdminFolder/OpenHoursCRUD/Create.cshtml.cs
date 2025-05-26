using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages.AdminFolder.OpenHoursCRUD
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
            return Page();
        }

        [BindProperty]
        public OpenHour OpenHour { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OpenHours.Add(OpenHour);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
