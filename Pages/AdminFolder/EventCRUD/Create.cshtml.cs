using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
using ZealandZooCase.Services;

namespace ZealandZooCase.Pages.AdminFolder.EventCrud
{
    public class CreateModel : PageModel
    {
        private readonly ZealandZooCase.Data.ZealandDBContext _context;
        private readonly ZealandService _service;

        public CreateModel(ZealandZooCase.Data.ZealandDBContext context, ZealandService service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult OnGet()
        {
        ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            return Page();
        }

        [BindProperty]
        public OurEvent OurEvent { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        
        public async Task<IActionResult> OnPostAsync()
        {
            var OurCustomer = _context.Users.Where(u => u.UserNewsletter == true).ToList();
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var AddEvent = _context.AllOurEvents.Add(OurEvent);
            await _context.SaveChangesAsync();
            foreach (var item in OurCustomer)
            {
                _service.SendMail(item, OurEvent);
            }

            return RedirectToPage("./Index");
        }
    }
}
