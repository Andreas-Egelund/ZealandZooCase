using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Pages
{
    public class SimpleViewModel : PageModel
    {
        private readonly ZealandDBContext _context;


        public SimpleViewModel(ZealandDBContext context)
        {
            _context = context;
        }


        public List<OurEvent> AllOfOurEvents { get; set; }





        public void OnGet()
        {

            AllOfOurEvents = _context.AllOurEvents.ToList();



        }
    }
}
