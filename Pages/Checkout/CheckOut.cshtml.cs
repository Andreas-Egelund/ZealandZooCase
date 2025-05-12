using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
using ZealandZooCase.Services;

namespace ZealandZooCase.Pages.Checkout
{
    public class CheckOutModel : PageModel
    {


        private readonly ZealandDBContext _dbContext;
        private readonly ZealandService _zealandService;


        public CheckOutModel(ZealandDBContext context, ZealandService service)
        {
            _dbContext = context;
            _zealandService = service;
        }


        public OurEvent CurrentEventBeingBought { get; set; }

        public User CurrentUser { get; set; }



        public void OnGet(int EventId)
        {

      
            CurrentEventBeingBought = _dbContext.AllOurEvents.FirstOrDefault(e => e.EventId == EventId);

            CurrentUser = _zealandService.SetCurrentUser();

        }


        public IActionResult OnPostCompletePurchase(int EventId)
        {

            var user = _zealandService.SetCurrentUser();

                _dbContext.AllEventSignups.Add(new AllEventSignup
                {
                    EventId = EventId,
                    UserId = user.UserId,
                    SignupDate = DateTime.Now
                });
                _dbContext.SaveChanges();

            return RedirectToPage("/HomePage");



        }
    }
}
