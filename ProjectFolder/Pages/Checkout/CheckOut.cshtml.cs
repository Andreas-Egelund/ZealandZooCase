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
        private readonly MailSenderService _mailSenderService;


        public CheckOutModel(ZealandDBContext context, ZealandService service, MailSenderService mailSenderService)
        {
            _dbContext = context;
            _zealandService = service;
            _mailSenderService = mailSenderService;
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


            CurrentEventBeingBought = _dbContext.AllOurEvents.FirstOrDefault(e => e.EventId == EventId);


            var user = _zealandService.SetCurrentUser();

                _dbContext.AllEventSignups.Add(new AllEventSignup
                {
                    EventId = EventId,
                    UserId = user.UserId,
                    SignupDate = DateTime.Now
                });
            _mailSenderService.SendMail(user, CurrentEventBeingBought);

            _dbContext.SaveChanges();



            return RedirectToPage("/HomePage");



        }
    }
}
